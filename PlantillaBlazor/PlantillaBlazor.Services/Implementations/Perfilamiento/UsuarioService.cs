using Encriptacion;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using PlantillaBlazor.Domain.Common.Email;
using PlantillaBlazor.Domain.Common.Options.AppConfig;
using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Constants;
using PlantillaBlazor.Domain.DTO.OTP;
using PlantillaBlazor.Domain.DTO.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Domain.Enums;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Email;
using PlantillaBlazor.Services.Interfaces.Encrypt;
using PlantillaBlazor.Services.Interfaces.Messages;
using PlantillaBlazor.Services.Interfaces.Otp;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PlantillaBlazor.Services.Implementations.Perfilamiento
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IOtpService _otpService;
        private readonly IEncryptService _encryptService;
        private readonly IAuditoriaService _auditoriaService;
        private readonly ISolicitudCambioContraseñaRepository _solicitudCambioContraseñaRepository;
        private readonly AppConfigOptions _appConfigOptions;
        private readonly IEmailService _emailService;
        private readonly IWhatsappMessageSender _whatsappMessageSender;
        private readonly NavigationManager _navigationManager;
        private readonly IRolService _rolService;
        private readonly ISessionService _sessionService;

        public UsuarioService
        (
            IUsuarioRepository usuarioRepository,
            IOtpService otpService,
            IEncryptService encryptService,
            IAuditoriaService auditoriaService,
            ISolicitudCambioContraseñaRepository solicitudCambioContraseñaRepository,
            IOptions<AppConfigOptions> appConfigOptions,
            IEmailService emailService,
            IWhatsappMessageSender whatsappMessageSender,
            NavigationManager navigationManager,
            IRolService rolService,
            ISessionService sessionService
        )
        {
            _usuarioRepository = usuarioRepository;
            _otpService = otpService;
            _encryptService = encryptService;
            _auditoriaService = auditoriaService;
            _solicitudCambioContraseñaRepository = solicitudCambioContraseñaRepository;
            _appConfigOptions = appConfigOptions.Value;
            _emailService = emailService;
            _navigationManager = navigationManager;
            _whatsappMessageSender = whatsappMessageSender;
            _rolService = rolService;
            _sessionService = sessionService;
        }

        public async Task<SolicitudRecuperacionClave> GetSolicitudContraseña(long idSolicitud)
        {
            var filtros = new List<Expression<Func<SolicitudRecuperacionClave, bool>>>();
            filtros.Add(s => s.Id == idSolicitud);

            var solicitudes = await _solicitudCambioContraseñaRepository.Get(filtros: filtros);

            return solicitudes.FirstOrDefault();
        }

        public async Task<Usuario> GetUsuarioById(long idUsuario)
        {
            var filtros = new List<Expression<Func<Usuario, bool>>>();

            filtros.Add(u => u.Id == idUsuario);

            var includes = new List<Expression<Func<Usuario, object>>>();

            includes.Add(u => u.Rol);

            var usuarios = await _usuarioRepository.Get(filtros: filtros, includes: includes);

            return usuarios.FirstOrDefault();
        }

        public async Task<Usuario> GetUsuarioByUser(string usuario)
        {
            var filtros = new List<Expression<Func<Usuario, bool>>>();

            filtros.Add(u => u.NombreUsuario == usuario);

            var usuarios = await _usuarioRepository.Get(filtros: filtros);

            return usuarios.FirstOrDefault();
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            var includes = new List<Expression<Func<Usuario, object>>>();

            includes.Add(u => u.Rol);

            return await _usuarioRepository.Get(includes: includes);
        }

        public async Task<ResultLogin<long>> ProcesarLoginUsuario(UserDTO usuarioDto, string ipAddress)
        {
            Usuario usuario = await GetUsuarioByUser(usuarioDto.Usuario);

            string mensajeError = "El usuario y/o las credenciales son incorrectas";

            if (usuario is null)
            {
                return ResultLogin<long>.Failure(mensajeError);
            }

            if (usuario!.TipoUsuario!.Equals("OAuth"))
            {
                return ResultLogin<long>.Failure("Acción no permitida");
            }

            if (!usuario.IsActive)
            {
                return ResultLogin<long>.Failure("El usuario está inactivo");
            }

            if (!PasswordHasher.VerifyPassword(usuarioDto.Password, usuario.Clave))
            {
                await _usuarioRepository.RegistrarAuditoriaLogin(new AuditoriaLoginUsuario()
                {
                    IdUsuario = usuario.Id,
                    Descripcion = "No exitoso, credenciales incorrectas",
                    IpLogin = ipAddress
                });

                //Intentos fallidos en los últimos 5 minutos
                long cantidadIntentosFallidos = await _usuarioRepository.GetCantidadIntentosFallidos(usuario.Id, 5);

                if (cantidadIntentosFallidos >= 3)
                {
                    usuario.IsActive = false;
                    await _usuarioRepository.InsertarUsuario(usuario);

                    await _usuarioRepository.RegistrarAuditoriaLogin(new AuditoriaLoginUsuario()
                    {
                        IdUsuario = usuario.Id,
                        Descripcion = "Se inactiva el usuario por realizar 3 o más intentos fallidos de ingreso en menos de 5 minutos",
                        IpLogin = ipAddress
                    });

                    return ResultLogin<long>.Failure(mensajeError);
                }
                else
                {
                    return ResultLogin<long>.Failure(mensajeError);
                }
            }

            if (usuario.MustChangePassword)
            {
                return ResultLogin<long>.Success(default, TipoRespuestaLogin.CAMBIO_CONTRASEÑA);
            }

            if (usuario.FechaUltimoCambioContraseña is not null)
            {
                //Días desde el último cambio de contraseña
                double diasDesdeUltimoCambioPass = (DateTime.Now - usuario.FechaUltimoCambioContraseña.GetValueOrDefault()).TotalDays;

                if (diasDesdeUltimoCambioPass >= 72) //Si supera 72 días, se debe hacer el cambio de contraseña
                {
                    await _usuarioRepository.RegistrarAuditoriaLogin(new AuditoriaLoginUsuario()
                    {
                        IdUsuario = usuario.Id,
                        Descripcion = $"Exitoso, pero el último cambio de contraseña fue hace {diasDesdeUltimoCambioPass} días, por lo que se debe hacer cambio de contraseña",
                        IpLogin = ipAddress
                    });

                    return ResultLogin<long>.Success(default, TipoRespuestaLogin.CAMBIO_CONTRASEÑA);
                }
            }

            AuditoriaLoginUsuario auditoria = new AuditoriaLoginUsuario()
            {
                IdUsuario = usuario.Id,
                Descripcion = $"Exitoso",
                IpLogin = ipAddress
            };

            await _usuarioRepository.RegistrarAuditoriaLogin(auditoria);

            return ResultLogin<long>.Success(auditoria.Id, Domain.Enums.TipoRespuestaLogin.EXITOSA);
        }

        public async Task<Result<long>> EnviarCodigoOTPLogin(long idUsuario, string ipAccion)
        {
            Usuario usuario = await _usuarioRepository.GetById(idUsuario);

            if (usuario is null) return Result<long>.Failure("Usuario no válido");

            if (string.IsNullOrEmpty(usuario.Email))
            {
                return Result<long>.Failure("El usuario no posee email registrado");
            }

            AuditoriaOtp auditoriaOtp = await _otpService.GenerarCodigoOtp(usuario.Id.ToString(), "Login", "Verificación otp login");

            string nombreCliente = _appConfigOptions.NombreCliente;
            string nombreAplicativo = _appConfigOptions.NombreAplicativo;

            #region Cuerpo correo

            string cuerpoCorreo = @$"<!DOCTYPE HTML
                          PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                        <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml""
                          xmlns:o=""urn:schemas-microsoft-com:office:office"">

                        <head>
                          <!--[if gte mso 9]>
                        <xml>
                          <o:OfficeDocumentSettings>
                            <o:AllowPNG/>
                            <o:PixelsPerInch>96</o:PixelsPerInch>
                          </o:OfficeDocumentSettings>
                        </xml>
                        <![endif]-->
                          <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                          <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                          <meta name=""x-apple-disable-message-reformatting"">
                          <!--[if !mso]><!-->
                          <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                          <title></title>

                          <style type=""text/css"">
                            @media only screen and (min-width: 620px) {{
                              .u-row {{
                                width: 600px !important;
                              }}

                              .u-row .u-col {{
                                vertical-align: top;
                              }}

                              .u-row .u-col-100 {{
                                width: 600px !important;
                              }}

                            }}

                            @media (max-width: 620px) {{
                              .u-row-container {{
                                max-width: 100% !important;
                                padding-left: 0px !important;
                                padding-right: 0px !important;
                              }}

                              .u-row .u-col {{
                                min-width: 320px !important;
                                max-width: 100% !important;
                                display: block !important;
                              }}

                              .u-row {{
                                width: 100% !important;
                              }}

                              .u-col {{
                                width: 100% !important;
                              }}

                              .u-col>div {{
                                margin: 0 auto;
                              }}
                            }}

                            body {{
                              margin: 0;
                              padding: 0;
                            }}

                            table,
                            tr,
                            td {{
                              vertical-align: top;
                              border-collapse: collapse;
                            }}

                            .ie-container table,
                            .mso-container table {{
                              table-layout: fixed;
                            }}

                            * {{
                              line-height: inherit;
                            }}

                            a[x-apple-data-detectors='true'] {{
                              color: inherit !important;
                              text-decoration: none !important;
                            }}

                            table,
                            td {{
                              color: #000000;
                            }}

                            #u_body a {{
                              color: #0000ee;
                              text-decoration: underline;
                            }}
                          </style>



                          <!--[if !mso]><!-->
                          <link href=""https://fonts.googleapis.com/css?family=Montserrat:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                          <!--<![endif]-->

                        </head>

                        <body class=""clean-body u_body""
                          style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f0f0f0;color: #000000"">
                          <!--[if IE]><div class=""ie-container""><![endif]-->
                          <!--[if mso]><div class=""mso-container""><![endif]-->
                          <table id=""u_body""
                            style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f0f0f0;width:100%""
                            cellpadding=""0"" cellspacing=""0"">
                            <tbody>
                              <tr style=""vertical-align: top"">
                                <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                                  <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #f0f0f0;""><![endif]-->



                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ddffe7;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div style=""background-color: #ddffe7;height: 100%;width: 100% !important;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                        <tr>
                                                          <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">

                                                            <img align=""center"" border=""0"" src=""https://i.ibb.co/VpXcjkn/image-1.png""
                                                              alt=""image"" title=""image""
                                                              style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 100%;max-width: 190px;""
                                                              width=""190"" />

                                                          </td>
                                                        </tr>
                                                      </table>

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 22px; font-weight: 700;"">
                                                        <span><span><span>Tu código de un solo uso es</span></span></span></h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><style>.v-button {{background: transparent !important;}}</style><![endif]-->
                                                      <div align=""center"">
                                                        <!--[if mso]><v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href="""" style=""height:42px; v-text-anchor:middle; width:216px;"" arcsize=""0%""  strokecolor=""#000000"" strokeweight=""2px"" fillcolor=""#ffffff""><w:anchorlock/><center style=""color:#000000;""><![endif]-->
                                                        <a href="""" target=""_blank"" class=""v-button""
                                                          style=""box-sizing: border-box;display: inline-block;text-decoration: none;-webkit-text-size-adjust: none;text-align: center;color: #000000; background-color: #ffffff; border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px; width:38%; max-width:100%; overflow-wrap: break-word; word-break: break-word; word-wrap:break-word; mso-border-alt: none;border-top-color: #000000; border-top-style: solid; border-top-width: 2px; border-left-color: #000000; border-left-style: solid; border-left-width: 2px; border-right-color: #000000; border-right-style: solid; border-right-width: 2px; border-bottom-color: #000000; border-bottom-style: solid; border-bottom-width: 2px;font-size: 18px;"">
                                                          <span style=""display:block;padding:10px 20px;line-height:120%;"">" + auditoriaOtp.Codigo + @"</span>
                                                        </a>
                                                        <!--[if mso]></center></v:roundrect><![endif]-->
                                                      </div>

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 16px; font-weight: 400;"">
                                                        Usa este código OTP para completar tu inicio de sesión en el aplicativo <b>" + nombreAplicativo + @"</b>.
                                                        El código es válido por 5 minutos.</h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 2px 0px 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 2px 0px 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 13px; font-weight: 400;"">
                                                        " + nombreCliente + @" | " + DateTime.Now.Year + @" | Todos los derechos reservados</h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>



                                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!--[if mso]></div><![endif]-->
                          <!--[if IE]></div><![endif]-->
                        </body>

                        </html>";

            #endregion

            return Result<long>.Failure("No posible enviar el código de verificación vía correo");
        }

        public async Task<Result<bool>> ValidarCodigoOTP(OtpDto otpCode, long idUsuario, string ipAccion)
        {
            Usuario usuario = await _usuarioRepository.GetById(idUsuario);

            if (usuario is null)
            {
                return Result<bool>.Failure("El usuario no existe");
            }

            AuditoriaOtp auditoriaOtp = await _otpService.GetUltimoOtpByProcess(idUsuario.ToString(), "Login");

            if (auditoriaOtp is null)
            {
                return Result<bool>.Failure("No es posible encontrar el código OTP");
            }

            string otpCliente = otpCode.C1 +
                                otpCode.C2 +
                                otpCode.C3 +
                                otpCode.C4 +
                                otpCode.C5 +
                                otpCode.C6
                                ;

            if (!auditoriaOtp.Codigo.Equals(otpCliente))
            {
                return Result<bool>.Failure("Código no válido");
            }

            //OTP mayor a 15 minutos
            if ((DateTime.Now - auditoriaOtp.FechaAdicion).TotalSeconds > (5 * 60))
            {
                return Result<bool>.Failure("El código ha expirado, por favor solicite uno nuevo");
            }

            auditoriaOtp.Estado = "Verificado";
            auditoriaOtp.FechaValidacion = DateTime.Now;

            await _otpService.CompletarOtp(auditoriaOtp);

            usuario.FechaUltimoIngreso = DateTime.Now;
            await _usuarioRepository.InsertarUsuario(usuario);

            return Result<bool>.Success(true);
        }

        public async Task<Result<long>> EnviarCodigoOTPPass(long idUsuario, string ipAccion)
        {
            Usuario usuario = await _usuarioRepository.GetById(idUsuario);

            if (usuario is null) return Result<long>.Failure("Usuario no válido");

            if (string.IsNullOrEmpty(usuario.Email))
            {
                return Result<long>.Failure("El usuario no posee email registrado");
            }

            AuditoriaOtp auditoriaOtp = await _otpService.GenerarCodigoOtp(usuario.Id.ToString(), "Recuperación de contraseña", "Verificación otp cambio de contraseña");

            string nombreCliente = _appConfigOptions.NombreCliente;
            string nombreAplicativo = _appConfigOptions.NombreAplicativo;

            #region Cuerpo correo

            string cuerpoCorreo = @$"<!DOCTYPE HTML
                          PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                        <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml""
                          xmlns:o=""urn:schemas-microsoft-com:office:office"">

                        <head>
                          <!--[if gte mso 9]>
                        <xml>
                          <o:OfficeDocumentSettings>
                            <o:AllowPNG/>
                            <o:PixelsPerInch>96</o:PixelsPerInch>
                          </o:OfficeDocumentSettings>
                        </xml>
                        <![endif]-->
                          <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                          <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                          <meta name=""x-apple-disable-message-reformatting"">
                          <!--[if !mso]><!-->
                          <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                          <title></title>

                          <style type=""text/css"">
                            @media only screen and (min-width: 620px) {{
                              .u-row {{
                                width: 600px !important;
                              }}

                              .u-row .u-col {{
                                vertical-align: top;
                              }}

                              .u-row .u-col-100 {{
                                width: 600px !important;
                              }}

                            }}

                            @media (max-width: 620px) {{
                              .u-row-container {{
                                max-width: 100% !important;
                                padding-left: 0px !important;
                                padding-right: 0px !important;
                              }}

                              .u-row .u-col {{
                                min-width: 320px !important;
                                max-width: 100% !important;
                                display: block !important;
                              }}

                              .u-row {{
                                width: 100% !important;
                              }}

                              .u-col {{
                                width: 100% !important;
                              }}

                              .u-col>div {{
                                margin: 0 auto;
                              }}
                            }}

                            body {{
                              margin: 0;
                              padding: 0;
                            }}

                            table,
                            tr,
                            td {{
                              vertical-align: top;
                              border-collapse: collapse;
                            }}

                            .ie-container table,
                            .mso-container table {{
                              table-layout: fixed;
                            }}

                            * {{
                              line-height: inherit;
                            }}

                            a[x-apple-data-detectors='true'] {{
                              color: inherit !important;
                              text-decoration: none !important;
                            }}

                            table,
                            td {{
                              color: #000000;
                            }}

                            #u_body a {{
                              color: #0000ee;
                              text-decoration: underline;
                            }}
                          </style>



                          <!--[if !mso]><!-->
                          <link href=""https://fonts.googleapis.com/css?family=Montserrat:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                          <!--<![endif]-->

                        </head>

                        <body class=""clean-body u_body""
                          style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f0f0f0;color: #000000"">
                          <!--[if IE]><div class=""ie-container""><![endif]-->
                          <!--[if mso]><div class=""mso-container""><![endif]-->
                          <table id=""u_body""
                            style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f0f0f0;width:100%""
                            cellpadding=""0"" cellspacing=""0"">
                            <tbody>
                              <tr style=""vertical-align: top"">
                                <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                                  <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #f0f0f0;""><![endif]-->



                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ddffe7;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div style=""background-color: #ddffe7;height: 100%;width: 100% !important;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                        <tr>
                                                          <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">

                                                            <img align=""center"" border=""0"" src=""https://i.ibb.co/VpXcjkn/image-1.png""
                                                              alt=""image"" title=""image""
                                                              style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 100%;max-width: 190px;""
                                                              width=""190"" />

                                                          </td>
                                                        </tr>
                                                      </table>

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 22px; font-weight: 700;"">
                                                        <span><span><span>Tu código de un solo uso es</span></span></span></h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><style>.v-button {{background: transparent !important;}}</style><![endif]-->
                                                      <div align=""center"">
                                                        <!--[if mso]><v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href="""" style=""height:42px; v-text-anchor:middle; width:216px;"" arcsize=""0%""  strokecolor=""#000000"" strokeweight=""2px"" fillcolor=""#ffffff""><w:anchorlock/><center style=""color:#000000;""><![endif]-->
                                                        <a href="""" target=""_blank"" class=""v-button""
                                                          style=""box-sizing: border-box;display: inline-block;text-decoration: none;-webkit-text-size-adjust: none;text-align: center;color: #000000; background-color: #ffffff; border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px; width:38%; max-width:100%; overflow-wrap: break-word; word-break: break-word; word-wrap:break-word; mso-border-alt: none;border-top-color: #000000; border-top-style: solid; border-top-width: 2px; border-left-color: #000000; border-left-style: solid; border-left-width: 2px; border-right-color: #000000; border-right-style: solid; border-right-width: 2px; border-bottom-color: #000000; border-bottom-style: solid; border-bottom-width: 2px;font-size: 18px;"">
                                                          <span style=""display:block;padding:10px 20px;line-height:120%;"">" + auditoriaOtp.Codigo + @"</span>
                                                        </a>
                                                        <!--[if mso]></center></v:roundrect><![endif]-->
                                                      </div>

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 16px; font-weight: 400;"">
                                                        Usa este código OTP para completar el proceso de reestablecimiento de contraseña en el aplicativo <b>" + nombreAplicativo + @"</b>.
                                                        El código es válido por 5 minutos.</h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>





                                  <div class=""u-row-container"" style=""padding: 2px 0px 0px;background-color: transparent"">
                                    <div class=""u-row""
                                      style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                      <div
                                        style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 2px 0px 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                        <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                        <div class=""u-col u-col-100""
                                          style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                          <div
                                            style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                            <!--[if (!mso)&(!IE)]><!-->
                                            <div
                                              style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                              <!--<![endif]-->

                                              <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                                cellspacing=""0"" width=""100%"" border=""0"">
                                                <tbody>
                                                  <tr>
                                                    <td
                                                      style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                      align=""left"">

                                                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                      <h1
                                                        style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 13px; font-weight: 400;"">
                                                        " + nombreCliente + @" | " + DateTime.Now.Year + @" | Todos los derechos reservados</h1>
                                                      <!--[if mso]></td></tr></table><![endif]-->

                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                              <!--[if (!mso)&(!IE)]><!-->
                                            </div><!--<![endif]-->
                                          </div>
                                        </div>
                                        <!--[if (mso)|(IE)]></td><![endif]-->
                                        <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                      </div>
                                    </div>
                                  </div>



                                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!--[if mso]></div><![endif]-->
                          <!--[if IE]></div><![endif]-->
                        </body>

                        </html>";

            #endregion

            EmailInfoDTO emailInfo = new EmailInfoDTO()
            {
                Asunto = $"Código OTP Reestablecimiento de Contraseña - {nombreAplicativo}",
                Mensaje = cuerpoCorreo,
                Descripcion = "Código OTP Reestablecimiento de contraseña",
                IdentificacionProceso = usuario.Id.ToString(),
                Pantalla = "Reestablecer contraseña"
            };

            emailInfo.Destinatarios.Add(usuario.Email);

            return Result<long>.Failure("No posible enviar el código de verificación vía correo");
        }

        public async Task<Result<bool>> ValidarCodigoOTPPass(OtpDto otpCode, long idUsuario, string ipAccion)
        {
            Usuario usuario = await _usuarioRepository.GetById(idUsuario);

            if (usuario is null)
            {
                return Result<bool>.Failure("El usuario no existe");
            }

            AuditoriaOtp auditoriaOtp = await _otpService.GetUltimoOtpByProcess(idUsuario.ToString(), "Recuperación de contraseña");

            if (auditoriaOtp is null)
            {
                return Result<bool>.Failure("No es posible encontrar el código OTP");
            }

            string otpCliente = otpCode.C1 +
                                otpCode.C2 +
                                otpCode.C3 +
                                otpCode.C4 +
                                otpCode.C5 +
                                otpCode.C6
                                ;

            if (!auditoriaOtp.Codigo.Equals(otpCliente))
            {
                return Result<bool>.Failure("Código no válido");
            }

            //OTP mayor a 15 minutos
            if ((DateTime.Now - auditoriaOtp.FechaAdicion).TotalSeconds > (5 * 60))
            {
                return Result<bool>.Failure("El código ha expirado, por favor solicite uno nuevo");
            }

            auditoriaOtp.Estado = "Verificado";
            auditoriaOtp.FechaValidacion = DateTime.Now;

            await _otpService.CompletarOtp(auditoriaOtp);

            return Result<bool>.Success(false);
        }

        public async Task<Result<bool>> GestionarSolicitudRecuperacionContraseña(string nombreUsuario, string ipAddress, string motivo)
        {
            if (string.IsNullOrEmpty(nombreUsuario)) return Result<bool>.Failure("Debes indicar una contraseña");

            Usuario usuario = await GetUsuarioByUser(nombreUsuario);

            if (usuario is null) return Result<bool>.Failure("El usuario no existe");

            SolicitudRecuperacionClave solicitud = new SolicitudRecuperacionClave()
            {
                Estado = "Pendiente",
                FechaAdicion = DateTime.Now,
                IdUsuario = usuario.Id,
                IpAccionInicial = ipAddress,
                IdUsuarioAdiciono = usuario.IdUsuarioAdiciono,
                MotivoCambioContraseña = motivo,
            };

            await _solicitudCambioContraseñaRepository.InsertarSolicitudRecuperacionContraseña(solicitud);

            if (solicitud.Id <= 0) return Result<bool>.Failure("No fue posible generar la solicitud de recuperación de contraseña");

            string base_url = _navigationManager.BaseUri;

            Dictionary<string, string> parametros = new Dictionary<string, string>()
            {
                {"idSolicitud", solicitud.Id.ToString()}
            };

            string parametrosEncriptados = _encryptService.EncriptarParametros(parametros);

            string url = base_url + $"CambioPass/{parametrosEncriptados}";

            string nombreCliente = _appConfigOptions.NombreCliente;
            string nombreAplicativo = _appConfigOptions.NombreAplicativo;
            string linkLogo = _appConfigOptions.LinkLogoCorreos;

            #region mensaje correo

            string mensajeCorreo = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                    <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
                    <head>
                    <!--[if gte mso 9]>
                    <xml>
                      <o:OfficeDocumentSettings>
                        <o:AllowPNG/>
                        <o:PixelsPerInch>96</o:PixelsPerInch>
                      </o:OfficeDocumentSettings>
                    </xml>
                    <![endif]-->
                      <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                      <meta name=""x-apple-disable-message-reformatting"">
                      <!--[if !mso]><!--><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                      <title></title>
  
                        <style type=""text/css"">
                          @media only screen and (min-width: 620px) {
                      .u-row {
                        width: 600px !important;
                      }
                      .u-row .u-col {
                        vertical-align: top;
                      }

                      .u-row .u-col-100 {
                        width: 600px !important;
                      }

                    }

                    @media (max-width: 620px) {
                      .u-row-container {
                        max-width: 100% !important;
                        padding-left: 0px !important;
                        padding-right: 0px !important;
                      }
                      .u-row .u-col {
                        min-width: 320px !important;
                        max-width: 100% !important;
                        display: block !important;
                      }
                      .u-row {
                        width: 100% !important;
                      }
                      .u-col {
                        width: 100% !important;
                      }
                      .u-col > div {
                        margin: 0 auto;
                      }
                    }
                    body {
                      margin: 0;
                      padding: 0;
                    }

                    table,
                    tr,
                    td {
                      vertical-align: top;
                      border-collapse: collapse;
                    }

                    p {
                      margin: 0;
                    }

                    .ie-container table,
                    .mso-container table {
                      table-layout: fixed;
                    }

                    * {
                      line-height: inherit;
                    }

                    a[x-apple-data-detectors='true'] {
                      color: inherit !important;
                      text-decoration: none !important;
                    }

                    table, td { color: #000000; } #u_body a { color: #0000ee; text-decoration: underline; } @media (max-width: 480px) { #u_content_image_1 .v-container-padding-padding { padding: 40px 10px 10px !important; } #u_content_image_1 .v-src-width { width: auto !important; } #u_content_image_1 .v-src-max-width { max-width: 50% !important; } #u_content_heading_1 .v-container-padding-padding { padding: 10px 10px 20px !important; } #u_content_heading_1 .v-font-size { font-size: 22px !important; } #u_content_heading_2 .v-container-padding-padding { padding: 40px 10px 10px !important; } #u_content_button_1 .v-container-padding-padding { padding: 30px 10px 40px !important; } #u_content_button_1 .v-size-width { width: 65% !important; } #u_content_text_deprecated_1 .v-container-padding-padding { padding: 10px 10px 20px !important; } }
                        </style>
  
  

                    <!--[if !mso]><!--><link href=""https://fonts.googleapis.com/css?family=Raleway:400,700&display=swap"" rel=""stylesheet"" type=""text/css""><!--<![endif]-->

                    </head>

                    <body class=""clean-body u_body"" style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f9f9ff;color: #000000"">
                      <!--[if IE]><div class=""ie-container""><![endif]-->
                      <!--[if mso]><div class=""mso-container""><![endif]-->
                      <table id=""u_body"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f9f9ff;width:100%"" cellpadding=""0"" cellspacing=""0"">
                      <tbody>
                      <tr style=""vertical-align: top"">
                        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #f9f9ff;""><![endif]-->
    
  
  
                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                      <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->
      
                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                      <div style=""background-color: #ffffff;height: 100%;width: 100% !important;"">
                      <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                    <table id=""u_content_image_1"" style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                      <tbody>
                        <tr>
                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:60px 10px 10px;font-family:'Raleway',sans-serif;"" align=""left"">
        
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                      <tr>
                        <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">
      
                          <img align=""center"" border=""0"" src=""https://i.ibb.co/YWvx2QM/images.png"" alt=""image"" title=""image"" style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 35%;max-width: 203px;"" width=""203"" class=""v-src-width v-src-max-width""/>
      
                        </td>
                      </tr>
                    </table>

                          </td>
                        </tr>
                      </tbody>
                    </table>

                    <table id=""u_content_heading_1"" style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                      <tbody>
                        <tr>
                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px 10px 30px;font-family:'Raleway',sans-serif;"" align=""left"">
        
                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                        <h1 class=""v-font-size"" style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-size: 28px; font-weight: 400;""><span><strong>¿Olvidaste tu contraseña?</strong></span></h1>
                      <!--[if mso]></td></tr></table><![endif]-->

                          </td>
                        </tr>
                      </tbody>
                    </table>

                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                      </div>
                    </div>
                    <!--[if (mso)|(IE)]></td><![endif]-->
                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                        </div>
                      </div>
                      </div>
  


  
  
                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                      <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->
      
                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                      <div style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                      <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;""><!--<![endif]-->
  
                    <table id=""u_content_heading_2"" style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                      <tbody>
                        <tr>
                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:40px 60px 10px;font-family:'Raleway',sans-serif;"" align=""left"">
        
                      <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                        <h1 class=""v-font-size"" style=""margin: 0px; line-height: 140%; text-align: left; word-wrap: break-word; font-size: 16px; font-weight: 400;""><span><span>Hola, " + usuario.Nombres + @". Te hemos enviado este correo en respuesta a tu solicitud de reestablecimiento de contraseña para el aplicativo " + nombreAplicativo + @".<br /><br />Para reestablecer tu contraseña, por favor da clic <a href=" + url + @" target=""_blank"">aquí</a></span></span></h1>
                      <!--[if mso]></td></tr></table><![endif]-->

                          </td>
                        </tr>
                      </tbody>
                    </table>


                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                      </div>
                    </div>
                    <!--[if (mso)|(IE)]></td><![endif]-->
                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                        </div>
                      </div>
                      </div>
  


  
  
                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                      <div class=""u-row"" style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->
      
                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                      <div style=""height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                      <!--[if (!mso)&(!IE)]><!--><div style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;""><!--<![endif]-->
  
                    <table id=""u_content_text_deprecated_1"" style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                      <tbody>
                        <tr>
                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px 100px 30px;font-family:'Raleway',sans-serif;"" align=""left"">
        
                      <div class=""v-font-size"" style=""font-size: 14px; line-height: 170%; text-align: center; word-wrap: break-word;"">
                        <p style=""font-size: 14px; line-height: 170%;"">" + nombreCliente + @" | " + DateTime.Now.Year + @" | Todos los derechos reservados</p>
                      </div>

                          </td>
                        </tr>
                      </tbody>
                    </table>

                    <table style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                      <tbody>
                        <tr>
                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:0px;font-family:'Raleway',sans-serif;"" align=""left"">
        
                      <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                        <tbody>
                          <tr style=""vertical-align: top"">
                            <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                              <span>&#160;</span>
                            </td>
                          </tr>
                        </tbody>
                      </table>

                          </td>
                        </tr>
                      </tbody>
                    </table>

                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                      </div>
                    </div>
                    <!--[if (mso)|(IE)]></td><![endif]-->
                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                        </div>
                      </div>
                      </div>
  


                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                        </td>
                      </tr>
                      </tbody>
                      </table>
                      <!--[if mso]></div><![endif]-->
                      <!--[if IE]></div><![endif]-->
                    </body>

                    </html>
                    ";

            #endregion

            return Result<bool>.Success(true);
        }

        private bool IsPasswordSecure(string password)
        {
            return Regex.Match(password, RegexConstants.REGEX_CONTRASEÑAS).Success;
        }

        public async Task<Result<bool>> CompletarSolicitudContraseña(long idSolicitud, string nuevaPass, string ipAddress)
        {
            if (string.IsNullOrEmpty(nuevaPass)) return Result<bool>.Failure("Debes indicar una contraseña");

            SolicitudRecuperacionClave solicitud = await _solicitudCambioContraseñaRepository.GetById(idSolicitud);

            if (solicitud is null)
            {
                return Result<bool>.Failure("La solicitud no existe");
            }

            if (solicitud.Estado.Equals("Finalizada"))
            {
                return Result<bool>.Failure("Esta solicitud ya fue finalizada");
            }

            double diferencia = (DateTime.Now - solicitud.FechaAdicion).TotalDays;

            if (diferencia > 1)
            {
                return Result<bool>.Failure("Esta solicitud de contraseña ya expiró");
            }

            Usuario usuario = await _usuarioRepository.GetById(solicitud.IdUsuario);

            if (usuario is null)
            {
                return Result<bool>.Failure("Usuario no válido");
            }

            if (!string.IsNullOrEmpty(usuario.Clave))
            {
                if (PasswordHasher.VerifyPassword(nuevaPass, usuario.Clave))
                {
                    return Result<bool>.Failure("Contraseña no válida, debes indicar otra");
                }
            }

            if (!IsPasswordSecure(nuevaPass))
            {
                return Result<bool>.Failure("La contraseña debe tener al menos 8 caracteres, un minúscula, una mayúscula, un número y un caracter especial");
            }

            string encriptada = PasswordHasher.HashPassword(nuevaPass);

            usuario.Clave = encriptada;
            usuario.MustChangePassword = false;
            usuario.FechaUltimoCambioContraseña = DateTime.Now;
            usuario.IsActive = true;

            if (!await _usuarioRepository.InsertarUsuario(usuario))
            {
                return Result<bool>.Failure("No es posible cambiar la contraseña del usuario");
            }

            solicitud.FechaFinalizacion = DateTime.Now;
            solicitud.Estado = "Finalizada";
            solicitud.IpAccionFinal = ipAddress;

            if (!await _solicitudCambioContraseñaRepository.InsertarSolicitudRecuperacionContraseña(solicitud))
            {
                return Result<bool>.Failure("No se pudo completar la solicitud");
            }

            await _sessionService.InhabilitarSessionsUsuario(usuario.Id, $"Se ha realizado el cambio de la contraseña mediante la solicitud {solicitud.Id}");

            #region Cuerpo correo

            string cuerpoCorreo = @"<!DOCTYPE HTML
                  PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml""
                  xmlns:o=""urn:schemas-microsoft-com:office:office"">

                <head>
                  <!--[if gte mso 9]>
                <xml>
                  <o:OfficeDocumentSettings>
                    <o:AllowPNG/>
                    <o:PixelsPerInch>96</o:PixelsPerInch>
                  </o:OfficeDocumentSettings>
                </xml>
                <![endif]-->
                  <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                  <meta name=""x-apple-disable-message-reformatting"">
                  <!--[if !mso]><!-->
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                  <title></title>

                  <style type=""text/css"">
                    @media only screen and (min-width: 620px) {
                      .u-row {
                        width: 600px !important;
                      }

                      .u-row .u-col {
                        vertical-align: top;
                      }

                      .u-row .u-col-100 {
                        width: 600px !important;
                      }

                    }

                    @media (max-width: 620px) {
                      .u-row-container {
                        max-width: 100% !important;
                        padding-left: 0px !important;
                        padding-right: 0px !important;
                      }

                      .u-row .u-col {
                        min-width: 320px !important;
                        max-width: 100% !important;
                        display: block !important;
                      }

                      .u-row {
                        width: 100% !important;
                      }

                      .u-col {
                        width: 100% !important;
                      }

                      .u-col>div {
                        margin: 0 auto;
                      }
                    }

                    body {
                      margin: 0;
                      padding: 0;
                    }

                    table,
                    tr,
                    td {
                      vertical-align: top;
                      border-collapse: collapse;
                    }

                    p {
                      margin: 0;
                    }

                    .ie-container table,
                    .mso-container table {
                      table-layout: fixed;
                    }

                    * {
                      line-height: inherit;
                    }

                    a[x-apple-data-detectors='true'] {
                      color: inherit !important;
                      text-decoration: none !important;
                    }

                    table,
                    td {
                      color: #000000;
                    }

                    @media (max-width: 480px) {
                      #u_content_image_1 .v-container-padding-padding {
                        padding: 40px 0px 0px !important;
                      }

                      #u_content_image_1 .v-src-width {
                        width: auto !important;
                      }

                      #u_content_image_1 .v-src-max-width {
                        max-width: 55% !important;
                      }

                      #u_content_heading_3 .v-font-size {
                        font-size: 18px !important;
                      }

                      #u_content_heading_4 .v-container-padding-padding {
                        padding: 40px 10px 0px !important;
                      }

                      #u_content_heading_4 .v-text-align {
                        text-align: center !important;
                      }

                      #u_content_divider_1 .v-container-padding-padding {
                        padding: 10px 10px 10px 125px !important;
                      }

                      #u_content_text_2 .v-container-padding-padding {
                        padding: 10px 10px 40px !important;
                      }

                      #u_content_text_2 .v-text-align {
                        text-align: center !important;
                      }

                      #u_content_text_deprecated_1 .v-container-padding-padding {
                        padding: 10px 10px 20px !important;
                      }
                    }
                  </style>



                  <!--[if !mso]><!-->
                  <link href=""https://fonts.googleapis.com/css?family=Raleway:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                  <!--<![endif]-->

                </head>

                <body class=""clean-body u_body""
                  style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #ffffff;color: #000000"">
                  <!--[if IE]><div class=""ie-container""><![endif]-->
                  <!--[if mso]><div class=""mso-container""><![endif]-->
                  <table
                    style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #ffffff;width:100%""
                    cellpadding=""0"" cellspacing=""0"">
                    <tbody>
                      <tr style=""vertical-align: top"">
                        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #ffffff;""><![endif]-->



                          <div class=""u-row-container"" style=""padding: 0px;background-color: #ffffff"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: #ffffff;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div style=""background-color: #ffffff;height: 100%;width: 100% !important;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"">
                                      <!--<![endif]-->

                                      <table id=""u_content_image_1"" style=""font-family:'Raleway',sans-serif;"" role=""presentation""
                                        cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:50px 0px 0px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                <tr>
                                                  <td class=""v-text-align"" style=""padding-right: 0px;padding-left: 0px;"" align=""center"">

                                                    <img align=""center"" border=""0"" src=""https://i.ibb.co/MkK4hdp/image-1.gif""
                                                      alt=""image"" title=""image""
                                                      style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 30%;max-width: 180px;""
                                                      width=""180"" class=""v-src-width v-src-max-width"" />

                                                  </td>
                                                </tr>
                                              </table>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <table id=""u_content_heading_3"" style=""font-family:'Raleway',sans-serif;"" role=""presentation""
                                        cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:10px 10px 0px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                              <h1 class=""v-text-align v-font-size""
                                                style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-size: 22px; font-weight: 400;"">
                                                <span><strong>Tu contraseña ha sido reestablecida</strong></span></h1>
                                              <!--[if mso]></td></tr></table><![endif]-->

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>





                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #f0f5f6;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""background-color: #f0f5f6;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->

                                      <table id=""u_content_heading_4"" style=""font-family:'Raleway',sans-serif;"" role=""presentation""
                                        cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:50px 60px 0px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                              <h1 class=""v-text-align v-font-size""
                                                style=""margin: 0px; line-height: 140%; text-align: left; word-wrap: break-word; font-size: 20px; font-weight: 400;"">
                                                <span><span><strong>Hola, " + usuario.Nombres + @"</strong></span></span></h1>
                                              <!--[if mso]></td></tr></table><![endif]-->

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <table id=""u_content_divider_1"" style=""font-family:'Raleway',sans-serif;"" role=""presentation""
                                        cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:10px 10px 10px 60px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <table height=""0px"" align=""left"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""38%""
                                                style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 2px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                <tbody>
                                                  <tr style=""vertical-align: top"">
                                                    <td
                                                      style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                      <span>&#160;</span>
                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <table id=""u_content_text_2"" style=""font-family:'Raleway',sans-serif;"" role=""presentation""
                                        cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:10px 60px 50px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <div class=""v-text-align v-font-size""
                                                style=""font-size: 14px; line-height: 140%; text-align: justify; word-wrap: break-word;"">
                                                <p style=""line-height: 140%;"">Mediante este correo te confirmamos que tu contraseña para el aplicativo <b>" + _appConfigOptions.NombreAplicativo + @"</b> ha
                                                  sido reestablecida en la fecha " + solicitud.FechaFinalizacion.GetValueOrDefault().ToString() + @".</p>
                                                <p style=""line-height: 140%;""> </p>
                                                <p style=""line-height: 140%;"">En caso de que no hayas sido tú, contáctate con el
                                                  administrador del sistema.</p>
                                              </div>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>





                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ecf0f1;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""background-color: #ecf0f1;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->

                                      <table id=""u_content_text_deprecated_1"" style=""font-family:'Raleway',sans-serif;""
                                        role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:10px 100px 30px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <div class=""v-text-align v-font-size""
                                                style=""font-size: 14px; line-height: 170%; text-align: center; word-wrap: break-word;"">
                                                <p style=""font-size: 14px; line-height: 170%;"">" + _appConfigOptions.NombreCliente + @" | " + DateTime.Now.Year + @" | Todos los
                                                  derechos reservados</p>
                                              </div>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <table style=""font-family:'Raleway',sans-serif;"" role=""presentation"" cellpadding=""0""
                                        cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td class=""v-container-padding-padding""
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:0px;font-family:'Raleway',sans-serif;""
                                              align=""left"">

                                              <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                <tbody>
                                                  <tr style=""vertical-align: top"">
                                                    <td
                                                      style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                      <span>&#160;</span>
                                                    </td>
                                                  </tr>
                                                </tbody>
                                              </table>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>



                          <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <!--[if mso]></div><![endif]-->
                  <!--[if IE]></div><![endif]-->
                </body>

                </html>";

            #endregion

            EmailInfoDTO emailInfo = new EmailInfoDTO()
            {
                Asunto = "Confirmación de reestablecimiento de contraseña",
                Descripcion = "Correo de Confirmación de reestablecimiento de contraseña",
                IdentificacionProceso = usuario.Id.ToString(),
                Mensaje = cuerpoCorreo,
                Pantalla = "Reestablecimiento de contraseña"
            };

            return Result<bool>.Success(true);
        }

        public async Task<Result<long>> InsertarUsuario(Usuario usuario, string ipAddress)
        {
            Result<bool> response = await ValidarInformacionUsuario(usuario);

            if (!response.IsSuccess) return Result<long>.Failure(response.Error);

            Usuario usuarioDb = await _usuarioRepository.GetById(usuario.Id);

            bool usuarioNuevo = usuarioDb is null;

            if (!usuarioNuevo)
            {
                usuario.NombreUsuario = usuarioDb.NombreUsuario;
                usuario.Clave = usuarioDb.Clave;
                usuario.Rol = await _rolService.GetRol(usuario.IdRol);
            }
            else
            {
                //El usuario debe cambiar la contraseña en su primer ingreso
                usuario.MustChangePassword = true;
                usuario.Clave = PasswordHasher.HashPassword(usuario.Clave);
            }

            if (!await _usuarioRepository.InsertarUsuario(usuario))
            {
                return Result<long>.Failure("No es posible guardar la información del usuario");
            }

            if (usuarioNuevo)
            {
                AuditoriaEvento auditoriaEvento = new AuditoriaEvento()
                {
                    Accion = "Registro de información de usuario nuevo",
                    Concepto = nameof(Usuario),
                    Descripcion = "Se ha registrado un nuevo usuario",
                    IdentificadorProceso = usuario.Id.ToString(),
                    IpAddress = ipAddress,
                    IdUsuarioAccion = usuario.IdUsuarioAdiciono.GetValueOrDefault(),
                };

                await _auditoriaService.RegistrarAuditoriaEvento(auditoriaEvento);

                string nombreCliente = _appConfigOptions.NombreCliente;
                string nombreAplicativo = _appConfigOptions.NombreAplicativo;
                string linkLogo = _appConfigOptions.LinkLogoCorreos;

                #region Correo bienvenida

                string correoBienvenida = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                                                    <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
                                                    <head>
                                                    <!--[if gte mso 9]>
                                                    <xml>
                                                      <o:OfficeDocumentSettings>
                                                        <o:AllowPNG/>
                                                        <o:PixelsPerInch>96</o:PixelsPerInch>
                                                      </o:OfficeDocumentSettings>
                                                    </xml>
                                                    <![endif]-->
                                                      <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                                                      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                                      <meta name=""x-apple-disable-message-reformatting"">
                                                      <!--[if !mso]><!--><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                                                      <title></title>
  
                                                        <style type=""text/css"">
                                                          @media only screen and (min-width: 570px) {
                                                      .u-row {
                                                        width: 550px !important;
                                                      }
                                                      .u-row .u-col {
                                                        vertical-align: top;
                                                      }

                                                      .u-row .u-col-50 {
                                                        width: 275px !important;
                                                      }

                                                      .u-row .u-col-100 {
                                                        width: 550px !important;
                                                      }

                                                    }

                                                    @media (max-width: 570px) {
                                                      .u-row-container {
                                                        max-width: 100% !important;
                                                        padding-left: 0px !important;
                                                        padding-right: 0px !important;
                                                      }
                                                      .u-row .u-col {
                                                        min-width: 320px !important;
                                                        max-width: 100% !important;
                                                        display: block !important;
                                                      }
                                                      .u-row {
                                                        width: calc(100% - 40px) !important;
                                                      }
                                                      .u-col {
                                                        width: 100% !important;
                                                      }
                                                      .u-col > div {
                                                        margin: 0 auto;
                                                      }
                                                    }
                                                    body {
                                                      margin: 0;
                                                      padding: 0;
                                                    }

                                                    table,
                                                    tr,
                                                    td {
                                                      vertical-align: top;
                                                      border-collapse: collapse;
                                                    }

                                                    p {
                                                      margin: 0;
                                                    }

                                                    .ie-container table,
                                                    .mso-container table {
                                                      table-layout: fixed;
                                                    }

                                                    * {
                                                      line-height: inherit;
                                                    }

                                                    a[x-apple-data-detectors='true'] {
                                                      color: inherit !important;
                                                      text-decoration: none !important;
                                                    }

                                                    table, td { color: #000000; } @media (max-width: 480px) { #u_content_image_1 .v-container-padding-padding { padding: 30px 10px 10px !important; } #u_content_image_1 .v-src-width { width: auto !important; } #u_content_image_1 .v-src-max-width { max-width: 66% !important; } #u_content_image_3 .v-src-width { width: auto !important; } #u_content_image_3 .v-src-max-width { max-width: 35% !important; } #u_content_image_2 .v-src-width { width: auto !important; } #u_content_image_2 .v-src-max-width { max-width: 55% !important; } #u_content_menu_1 .v-container-padding-padding { padding: 10px !important; } }
                                                        </style>
  
  

                                                    <!--[if !mso]><!--><link href=""https://fonts.googleapis.com/css?family=Cabin:400,700&display=swap"" rel=""stylesheet"" type=""text/css""><link href=""https://fonts.googleapis.com/css?family=Lato:400,700&display=swap"" rel=""stylesheet"" type=""text/css""><!--<![endif]-->

                                                    </head>

                                                    <body class=""clean-body u_body"" style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #e0e5eb;color: #000000"">
                                                      <!--[if IE]><div class=""ie-container""><![endif]-->
                                                      <!--[if mso]><div class=""mso-container""><![endif]-->
                                                      <table style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #e0e5eb;width:100%"" cellpadding=""0"" cellspacing=""0"">
                                                      <tbody>
                                                      <tr style=""vertical-align: top"">
                                                        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                                                        <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #e0e5eb;""><![endif]-->
    

                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: transparent;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 0px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                        <tbody>
                                                          <tr style=""vertical-align: top"">
                                                            <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                              <span>&#160;</span>
                                                            </td>
                                                          </tr>
                                                        </tbody>
                                                      </table>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: #ffffff;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""275"" style=""width: 275px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-50"" style=""max-width: 320px;min-width: 275px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table id=""u_content_image_1"" style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                      <tr>
                                                        <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">
      
                                                          <img align=""center"" border=""0"" src=""" + linkLogo + @""" alt=""Image"" title=""Image"" style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 74%;max-width: 188.7px;"" width=""188.7"" class=""v-src-width v-src-max-width""/>
      
                                                        </td>
                                                      </tr>
                                                    </table>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""275"" style=""width: 275px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-50"" style=""max-width: 320px;min-width: 275px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #f8f9fa;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: #d5827c;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table id=""u_content_image_3"" style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                      <tr>
                                                        <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">
      
                                                          <img align=""center"" border=""0"" src=""https://i.ibb.co/K24pPrz/user.png"" alt=""Image"" title=""Image"" style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 26%;max-width: 137.8px;"" width=""137.8"" class=""v-src-width v-src-max-width""/>
      
                                                        </td>
                                                      </tr>
                                                    </table>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #d5827c;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: #d5827c;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:15px 10px 10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <h1 style=""margin: 0px; color: #ffffff; line-height: 140%; text-align: center; word-wrap: break-word; font-weight: normal; font-family: book antiqua,palatino; font-size: 35px;"">
                                                        <div>Bienvenido, " + usuario.NombreUsuario + @"</div>
                                                      </h1>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:0px 10px 10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <h3 style=""margin: 0px; color: #ffffff; line-height: 140%; text-align: center; word-wrap: break-word; font-weight: normal; font-family: book antiqua,palatino; font-size: 18px;"">
                                                        <div>tu cuenta está lista para usarse</div>
                                                      </h3>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #d5827c;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: #d5827c;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table id=""u_content_image_2"" style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                      <tr>
                                                        <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">
      
                                                          <img align=""center"" border=""0"" src=""https://i.ibb.co/2vfkNTr/image-1.png"" alt=""Image"" title=""Image"" style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 39%;max-width: 206.7px;"" width=""206.7"" class=""v-src-width v-src-max-width""/>
      
                                                        </td>
                                                      </tr>
                                                    </table>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px 40px 20px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <div style=""color: #4b4a4a; line-height: 140%; text-align: center; word-wrap: break-word;"">
                                                        <p style=""font-size: 14px; line-height: 140%;""><span style=""font-size: 12px; line-height: 16.8px;"">Tu nuevo usuario para el aplicativo <b>" + nombreAplicativo + @"</b> está listo para ser usado.</span></p>
                                                      </div>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: #ffffff;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table id=""u_content_menu_1"" style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                    <div class=""menu"" style=""text-align:center"">
                                                    <!--[if (mso)|(IE)]><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" align=""center""><tr><![endif]-->

                                                    <!--[if (mso)|(IE)]></tr></table><![endif]-->
                                                    </div>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px 10px 15px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <div style=""color: #7e8c8d; line-height: 140%; text-align: center; word-wrap: break-word;"">
                                                        <p style=""font-size: 14px; line-height: 140%;"">© " + DateTime.Now.Year + @" " + nombreCliente + @". Todos los derechos reservados.</p>
                                                      </div>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>



                                                    <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                                      <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 550px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                                        <div style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:550px;""><tr style=""background-color: transparent;""><![endif]-->
      
                                                    <!--[if (mso)|(IE)]><td align=""center"" width=""550"" style=""width: 550px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                                    <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 550px;display: table-cell;vertical-align: top;"">
                                                      <div style=""height: 100%;width: 100% !important;"">
                                                      <!--[if (!mso)&(!IE)]><!--><div style=""height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""><!--<![endif]-->
  
                                                    <table style=""font-family:'Lato',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0"">
                                                      <tbody>
                                                        <tr>
                                                          <td class=""v-container-padding-padding"" style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Lato',sans-serif;"" align=""left"">
        
                                                      <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 0px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                        <tbody>
                                                          <tr style=""vertical-align: top"">
                                                            <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                              <span>&#160;</span>
                                                            </td>
                                                          </tr>
                                                        </tbody>
                                                      </table>

                                                          </td>
                                                        </tr>
                                                      </tbody>
                                                    </table>

                                                      <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->
                                                      </div>
                                                    </div>
                                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                                          <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                                        </div>
                                                      </div>
                                                    </div>


                                                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                                                        </td>
                                                      </tr>
                                                      </tbody>
                                                      </table>
                                                      <!--[if mso]></div><![endif]-->
                                                      <!--[if IE]></div><![endif]-->
                                                    </body>

                                                    </html>
                                                    ";

                #endregion

            }
            else
            {
                if (!usuario.IsActive)
                {
                    await _sessionService.InhabilitarSessionsUsuario(usuario.Id, $"Se ha inactivado el usuario en la fecha {DateTime.Now.ToString()}");
                }

                AuditoriaEvento auditoriaEvento = new AuditoriaEvento()
                {
                    Accion = "Actualizacion de información de usuario",
                    Concepto = nameof(Usuario),
                    Descripcion = "Se ha actualizado la información del usuario",
                    IdentificadorProceso = usuario.Id.ToString(),
                    IpAddress = ipAddress,
                    IdUsuarioAccion = usuario.IdUsuarioUltimaActualizacion.GetValueOrDefault(),
                };

                await _auditoriaService.RegistrarAuditoriaEvento(auditoriaEvento);
            }

            return Result<long>.Success(usuario.Id);
        }

        public async Task ProcesarIngreso(Usuario usuario, string ipAddress, string descripcion)
        {
            AuditoriaLoginUsuario auditoria = new AuditoriaLoginUsuario()
            {
                Descripcion = descripcion,
                IpLogin = ipAddress,
                IdUsuario = usuario.Id,
                FechaAdicion = DateTime.Now
            };

            Usuario tempUser = await _usuarioRepository.GetById(usuario.Id);
            tempUser.FechaUltimoIngreso = DateTime.Now;
            await _usuarioRepository.InsertarUsuario(tempUser);

            await _usuarioRepository.RegistrarAuditoriaLogin(auditoria);
        }

        public async Task<bool> RegistrarAuditoriaCierreSesion(long idAuditoria, string ipAddress, string motivo)
        {
            await _sessionService.InhabilitarSessionByIdAuditoria(idAuditoria, motivo);

            AuditoriaLoginUsuario auditoria = new AuditoriaLoginUsuario();

            auditoria.Id = idAuditoria;
            auditoria.IpCierreSesion = ipAddress;
            auditoria.MotivoCierreSesion = motivo;

            return await _usuarioRepository.RegistrarAuditoriaCierreSesion(auditoria);
        }

        public async Task<IEnumerable<Modulo>> GetModulosUsuario(long idUsuario)
        {
            Usuario usuario = await _usuarioRepository.GetById(idUsuario);

            if (usuario is null) return new List<Modulo>();

            return await _rolService.GetModulosRol(usuario.IdRol);
        }

        public async Task<SolicitudRecuperacionClave> GenerarSolicitudContraseña(long idUsuario, string ipAddress, string motivo)
        {
            SolicitudRecuperacionClave solicitud = new SolicitudRecuperacionClave()
            {
                Estado = "Pendiente",
                FechaAdicion = DateTime.Now,
                IdUsuario = idUsuario,
                IpAccionInicial = ipAddress,
                IdUsuarioAdiciono = idUsuario,
                MotivoCambioContraseña = motivo
            };

            await _solicitudCambioContraseñaRepository.InsertarSolicitudRecuperacionContraseña(solicitud);

            return solicitud;
        }

        public async Task<Result<bool>> ValidarInformacionUsuario(Usuario usuario)
        {
            #region Validacion

            if (string.IsNullOrEmpty(usuario.Nombres))
            {
                return Result<bool>.Failure("Debe indicar los nombres del usuario");
            }

            if (string.IsNullOrEmpty(usuario.Apellidos))
            {
                return Result<bool>.Failure("Debe indicar los apellidos del usuario");
            }

            if (string.IsNullOrEmpty(usuario.NombreUsuario))
            {
                return Result<bool>.Failure("Debe indicar el nombre de usuario");
            }

            if (string.IsNullOrEmpty(usuario.Email))
            {
                return Result<bool>.Failure("Debe indicar el email del usuario");
            }

            if (!Regex.Match(usuario.Email, RegexConstants.EMAIL_REGEX).Success)
            {
                return Result<bool>.Failure("Dirección de email no válida");
            }

            if (usuario.IdRol <= 0)
            {
                return Result<bool>.Failure("Debe indicar el rol del usuario");
            }

            #endregion

            //Se valida que el nombre de usuario no exista

            var filtros = new List<Expression<Func<Usuario, bool>>>();
            filtros.Add(u => u.NombreUsuario == usuario.NombreUsuario && u.Id != usuario.Id);

            var tempU = await _usuarioRepository.Get(filtros: filtros);

            if (tempU.Count() > 0)
            {
                return Result<bool>.Failure("Ya existe un usuario con el nombre de usuario indicado");
            }

            return Result<bool>.Success(true);
        }

        public async Task<bool> InactivarUsuariosNoActivos(int diasDesdeUltimoLoggeo)
        {
            return await _usuarioRepository.InactivarUsuariosNoActivos(diasDesdeUltimoLoggeo);
        }

        public async Task<bool> InhabilitarSolicitudesCambioPassCaducadas(int dias)
        {
            return await _solicitudCambioContraseñaRepository.InhabilitarSolicitudesCaducadas(dias);
        }

        public async Task<Result<SessionDTO>> ProcesarIngresoUsuarioOAuth(OAuthUserDTO user)
        {
            var usuario = await GetUsuarioByUser(user.UserName!);

            if (usuario is null)
            {
                //Default role

                string[] gruposAdmin = ["Administradores", "Desarrollo", "Gerencia", "Proyectos"];

                var roles = await _rolService.GetRoles();
                Rol defaultRole;

                if (user.Grupos.Any(g => gruposAdmin.Any(ga => ga.Equals(g))))
                {
                    defaultRole = roles.FirstOrDefault(r => r.Id == 1)!;
                }
                else
                {
                    defaultRole = roles.FirstOrDefault(r => r.Nombre.Equals("Default"))!;
                }

                //Se crea el usuario
                usuario = new Usuario()
                {
                    Nombres = user.Name,
                    NombreUsuario = user.UserName,
                    IsActive = true,
                    IdRol = defaultRole is null ? 1 : defaultRole.Id,
                    TipoUsuario = "OAuth",
                    MustChangePassword = false,
                    Email = user.Email!
                };

                await _usuarioRepository.InsertarUsuario(usuario);
            }

            AuditoriaLoginUsuario auditoria = new AuditoriaLoginUsuario()
            {
                Descripcion = "Ingreso de usuario mediante OAuth",
                IpLogin = user.IpAddress,
                IdUsuario = usuario.Id,
                FechaAdicion = DateTime.Now
            };

            usuario.FechaUltimoIngreso = DateTime.Now;

            await _usuarioRepository.InsertarUsuario(usuario);

            await _usuarioRepository.RegistrarAuditoriaLogin(auditoria);

            var session = new Session()
            {
                FechaUltimoIngreso = DateTime.Now,
                Host = user.Host,
                IdAuditoriaLogin = auditoria.Id,
                IdUsuario = usuario.Id,
                IpAddress = user.IpAddress,
                IsActive = true,
                RememberMe = true,
                TipoUsuario = "OAuth"
            };

            var resultSession = await _sessionService.ProcesarIngresoUsuario(session);

            if (!resultSession.IsSuccess)
            {
                return Result<SessionDTO>.Failure("No es posible crear la sesión");
            }

            return Result<SessionDTO>.Success(new SessionDTO()
            {
                IdAuditoria = auditoria.Id,
                IdSession = session.Id,
                IdUsuario = usuario.Id,
                RememberMe = true
            });
        }

        public async Task<Result<bool>> ActualizarInfoUsuarioOAuth(Usuario usuario)
        {
            var tempUser = await _usuarioRepository.GetById(usuario.Id);

            if (tempUser is null) return Result<bool>.Failure("Usuario no válido");

            if (!tempUser!.TipoUsuario!.Equals("OAuth")) return Result<bool>.Failure("Usuario no válido");

            if (!await _usuarioRepository.InsertarUsuario(usuario)) return Result<bool>.Failure("No es posible actualizar info de usuario");

            return Result<bool>.Success(true);
        }

        public async Task<AuditoriaLoginUsuario> GetAuditoriaLogin(long id)
        {
            return await _usuarioRepository.GetAuditoriaLoginUsuario(id);
        }
    }
}
