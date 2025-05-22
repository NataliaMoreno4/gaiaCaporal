using PlantillaBlazor.Services.Interfaces.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlantillaBlazor.Domain.Common.Email;
using Microsoft.Extensions.Options;
using PlantillaBlazor.Domain.Common.Options.Email;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Services.Interfaces.Auditoria;

namespace PlantillaBlazor.Services.Implementations.Email
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IEmailService"/>
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpOptions _smtpOptions;
        private readonly IAuditoriaService _auditoriaService;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger,
            IOptions<SmtpOptions> smtpOptionsService,
            IAuditoriaService auditoriaService
        )
        {
            _configuration = configuration;
            _logger = logger;
            _smtpOptions = smtpOptionsService.Value;
            _auditoriaService = auditoriaService;
        }

        public async Task<bool> EnviarCorreo(EmailInfoDTO emailInfo)
        {
            //Auditoria
            AuditoriaEnvioEmail auditoriaEnvioEmail = new AuditoriaEnvioEmail();

            if (emailInfo.Destinatarios.Count <= 0) return false;

            if (emailInfo.Destinatarios.All(d => string.IsNullOrEmpty(d))) return false;

            try
            {
                auditoriaEnvioEmail.EmailEmisor = _smtpOptions.EmailFrom;
                auditoriaEnvioEmail.EmailDestinatario = string.Join(";", emailInfo.Destinatarios);
                auditoriaEnvioEmail.EmailCC = string.Join(";", emailInfo.Cc);
                auditoriaEnvioEmail.EmailBCC = string.Join(";", emailInfo.Bcc);
                auditoriaEnvioEmail.Asunto = emailInfo.Asunto;
                auditoriaEnvioEmail.MensajeHTML = emailInfo.Mensaje;
                auditoriaEnvioEmail.Concepto = emailInfo.Descripcion;
                auditoriaEnvioEmail.NumeroIdentificacionProceso = emailInfo.IdentificacionProceso;
                auditoriaEnvioEmail.Pantalla = emailInfo.Pantalla;
                auditoriaEnvioEmail.Host = _smtpOptions.EmailHost;
                auditoriaEnvioEmail.Puerto = _smtpOptions.EmailPort;
                auditoriaEnvioEmail.SslEnabled = _smtpOptions.EmailSSLEnabled;


                MailMessage emailsend = new MailMessage();
                //Bandeja de correo que enviará el correo
                emailsend.From = new MailAddress(_smtpOptions.EmailFrom);
                //Destinatarios del correo
                foreach (var d in emailInfo.Destinatarios)
                {
                    if (!string.IsNullOrEmpty(d))
                    {
                        emailsend.To.Add(new MailAddress(d));
                    }
                }
                //Copias del correo
                foreach (var cc in emailInfo.Cc)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        emailsend.CC.Add(new MailAddress(cc));
                    }
                }
                //Copias ocultas del correo
                emailInfo.Bcc.Add(_smtpOptions.EmailBCC);

                foreach (var bcc in emailInfo.Bcc)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        emailsend.Bcc.Add(new MailAddress(bcc));
                    }
                }

                emailsend.Subject = emailInfo.Asunto;
                emailsend.IsBodyHtml = true;
                emailsend.Body = emailInfo.Mensaje;
                emailsend.Priority = MailPriority.Normal;

                if (emailInfo.Adjuntos.Count > 0)
                {
                    auditoriaEnvioEmail.ListaAdjuntosEmail = emailInfo.Adjuntos.Select(a => new AuditoriaAdjuntoEmail()
                    {
                        NombreAdjunto = a.NombreArchivo,
                        FechaAdicion = DateTime.Now,
                        RutaAbsolutaAdjunto = a.RutaAdjunto
                    }).ToList();

                    foreach (var anexo in emailInfo.Adjuntos)
                    {
                        if (File.Exists(anexo.RutaAdjunto))
                        {
                            Attachment attachment = new Attachment(anexo.RutaAdjunto, MediaTypeNames.Application.Octet);

                            if (!string.IsNullOrEmpty(anexo.NombreArchivo))
                            {
                                attachment.ContentDisposition.FileName = $"{anexo.NombreArchivo}{Path.GetExtension(anexo.RutaAdjunto)}";
                            }

                            emailsend.Attachments.Add(attachment);
                        }
                    }
                }

                SmtpClient smtp = new SmtpClient(_smtpOptions.EmailHost);
                smtp.Host = _smtpOptions.EmailHost;
                smtp.Port = _smtpOptions.EmailPort;
                smtp.EnableSsl = _smtpOptions.EmailSSLEnabled;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_smtpOptions.EmailFrom, _smtpOptions.EmailPass);

                //INTENTOS DE ENVIO DE CORREO
                Boolean enviado = false;
                int intentos = 0;
                while (!enviado && intentos < 3)
                {
                    try
                    {
                        smtp.Send(emailsend);
                        enviado = true;
                        emailsend.Dispose();

                        auditoriaEnvioEmail.FueEnviado = true;
                    }
                    catch (Exception exe)
                    {
                        _logger.LogError(exe, $"Error al enviar correo");
                        auditoriaEnvioEmail.DescripcionError = $"{exe.Message} {exe.InnerException}";
                        auditoriaEnvioEmail.FueEnviado = false;
                    }

                    auditoriaEnvioEmail.Id = 0;
                    await _auditoriaService.RegistrarAuditoriaEnvioEmail(auditoriaEnvioEmail);

                    Thread.Sleep(3000);
                    intentos++;
                }
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al enviar correo");
            }

            return auditoriaEnvioEmail.FueEnviado;
        }
    }
}
