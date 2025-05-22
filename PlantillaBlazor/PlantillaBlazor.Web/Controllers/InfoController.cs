using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using PlantillaBlazor.Web.Entities.Client;
using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Auditoria;

namespace PlantillaBlazor.Web.Controllers
{
    /// <summary>
    /// Controlador mediante el cual el cliente interactuará con el servidor para obtener datos como la dirección IP o eventos de navegación
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly ILogger<InfoController> _logger;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IUsuarioService _userService;
        private readonly AuthenticationStateProvider _authStateProvider;


        public InfoController(
            ILogger<InfoController> logger,
            IAuditoriaService auditoriaService,
            IUsuarioService userService,
            AuthenticationStateProvider authStateProvider
            )
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
            _authStateProvider = authStateProvider;
            _userService = userService;
        }

        /// <summary>
        /// Retorna la dirección IP del cliente que realiza la petición
        /// </summary>
        /// <returns>Dirección IP del cliente</returns>
        [HttpGet]
        [Route("ipaddress")]
        public async Task<string> GetIpAddress()
        {
            try
            {
                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                if (remoteIpAddress != null)
                    return remoteIpAddress.ToString();
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, "Error al obtener ipaddress en InfoControler");
            }
            return string.Empty;
        }

        /// <summary>
        /// Recieve distintos datos del cliente envíados por éste mediante un modelo <see cref="DatosClienteModel"/> para registrarlos como auditoría de navegación
        /// </summary>
        /// <param name="datosCliente">Modelo <see cref="DatosClienteModel"/></param>
        [HttpPost]
        [Route("sendClientInfo")]
        public async Task getClientInfo(DatosClienteModel datosCliente)
        {
            try
            {
                var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                Usuario user = await _userService.GetUsuarioByUser(datosCliente.actualUser);

                string usuarioAccion = user is null ? "" : user.NombreUsuario;
                string idUsuario = user is null ? "" : user.Id.ToString();

                string ubicacion = "Permitida";

                if (datosCliente.Ubicacion is null)
                {
                    ubicacion = "No permitida";
                    datosCliente.Ubicacion = new DatosClienteModel.UbicacionModel() { Latitud = -1, Longitud = -1 };
                }

                AuditoriaNavegacion auditoria = new AuditoriaNavegacion()
                {
                    UserAgent = datosCliente.UserAgent,
                    Navegador = datosCliente.Navegador,
                    AltoPantalla = datosCliente.ResolucionPantalla.Alto.ToString(),
                    AnchoPantalla = datosCliente.ResolucionPantalla.Ancho.ToString(),
                    CookiesHabilitadas = datosCliente.CookiesHabilitadas.ToString(),
                    Idioma = datosCliente.Idioma,
                    IpAddress = remoteIpAddress.ToString(),
                    Latitud = datosCliente.Ubicacion.Latitud.ToString(),
                    Longitud = datosCliente.Ubicacion.Longitud.ToString(),
                    NombreSO = datosCliente.SistemaOperativo.Nombre,
                    PlataformaNavegador = datosCliente.PlataformaNavegador,
                    ProfundidadColor = datosCliente.ProfundidadColor.ToString(),
                    UrlActual = datosCliente.Url,
                    IdUsuarioAccion = usuarioAccion,
                    VersionNavegador = datosCliente.VersionNavegador,
                    VersionSO = datosCliente.SistemaOperativo.Version,
                    IsLocationPermitted = ubicacion
                };

                await _auditoriaService.RegistrarAuditoriaNavegacion(auditoria);
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, "Error al procesar clientinfo");
            }
        }

    }
}
