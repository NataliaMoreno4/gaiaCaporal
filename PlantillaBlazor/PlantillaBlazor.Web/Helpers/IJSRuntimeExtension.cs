using Microsoft.JSInterop;
using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Services.Interfaces.Auditoria;
using Serilog;

namespace PlantillaBlazor.Web.Helpers
{
    public static class IJSRuntimeExtension
    {
        public static IAuditoriaService _auditoriaService { get; private set; }

        public static void setAuditoriaService(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }
        /// <summary>
        /// Muestra en pantalla una alerta <c>Sweet Alert</c> común
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta se será visible</param>
        /// <param name="footer">Contenido del footer de la alera</param>
        public static async Task DisplaySwal(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje, string footer = "")
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("displaySwal", titulo, mensaje, footer, tipoMensaje.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar swal");
            }
        }
        /// <summary>
        /// Muestra en pantalla una alerta <c>Sweet Alert</c> en la cual se puede mostrar contenido <c>HTML</c> en el mensaje
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="html">Mensaje de la alerta en <c>HTML</c></param>
        /// <param name="tipoMensaje">Tipo de alerta se será visible</param>
        /// <param name="footer">Contenido del footer de la alerta</param>
        public static async Task DisplaySwalWithHtml(this IJSRuntime jsRuntime, string titulo, string html, TipoMensajeSweetAlert tipoMensaje, string footer = "")
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("displaySwalWithHtml", titulo, html, footer, tipoMensaje.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar alerta con html");
            }
        }
        /// <summary>
        /// Muestra una alerta Sweet Alerta de confirmación al usuario, en donde se tiene una elección SI/NO
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task<bool> DisplayConfirmationAlert(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje, string footer = "")
        {
            try
            {
                return await jsRuntime.InvokeAsync<bool>("customConfirmSwal", titulo, mensaje, tipoMensaje.ToString(), footer);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar alerta confirm");
            }

            return false;
        }
        /// <summary>
        /// Muestra una alerta de confirmación al usuario, en donde se espera una acción del usuario para continuar 
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta</param>
        /// <param name="footer">Contenido del footer de la alerta</param>
        public static async Task<bool> DisplayWaitingAlert(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje, string footer = "")
        {
            try
            {
                return await jsRuntime.InvokeAsync<bool>("customConfirmSuccessSwal", titulo, mensaje, tipoMensaje.ToString(), footer);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error mostrar waiting alert");
            }

            return false;
        }
        /// <summary>
        /// Muestra una alerta Sweet Alert de cargando
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        public static async Task DisplayAlertLoading(this IJSRuntime jsRuntime, string titulo, string mensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("LoadingSwal", titulo, mensaje);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Cierra cualquier alerta Sweet Alerta en pantalla
        /// </summary>
        public static async Task CloseAlerts(this IJSRuntime jsRuntime)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Swal.close");
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al cerrar alertas");
            }
        }
        /// <summary>
        /// Realiza la descarga de un archivo al cliente
        /// </summary>
        /// <param name="ruta">Ruta absoluta del archivo que se va a descargar</param>
        /// <param name="nombre">Nombre con el que será descargado el archivo</param>
        public static async Task<Result<bool>> DescargarArchivo(this IJSRuntime jsRuntime, string ruta, string nombre)
        {
            if (!File.Exists(ruta)) return Result<bool>.Failure("La ruta especificada no existe");

            try
            {
                FileInfo fi = new FileInfo(ruta);
                string rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");
                Directory.CreateDirectory(rutaDestino);

                string rutaFinal = Path.Combine(rutaDestino, $"TempFile_{Guid.NewGuid().ToString("N")}{Path.GetExtension(ruta)}");

                if (!ruta.Equals(rutaFinal))
                    File.Copy(ruta, rutaFinal);

                string rutaCliente = rutaFinal.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "").Replace("\\", "/");

                await jsRuntime.InvokeVoidAsync("downloadURI", rutaCliente, nombre + fi.Extension);

                string usuario = "";

                try
                {
                    usuario = await jsRuntime.InvokeAsync<string>("getActualUser");
                }
                catch (Exception exe)
                {

                }

                AuditoriaDescargaArchivo auditoria = new AuditoriaDescargaArchivo()
                {
                    ExtensionArchivo = fi.Extension,
                    IpAddress = await jsRuntime.InvokeAsync<string>("getIpAddress")
                    .ConfigureAwait(true),
                    NombreArchivo = nombre + fi.Extension,
                    RutaDescargada = rutaCliente,
                    RutaOriginal = ruta,
                    Usuario = usuario,
                    PesoArchivo = new FileInfo(ruta).Length
                };

                await _auditoriaService.RegistrarAuditoriaDescargaArchivo(auditoria);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al descargar archivo");
                return Result<bool>.Failure("Ocurrió un error al procesar el archivo");
            }

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Dado un archivo con ruta absoluta dentro del servidor, genera una ruta cliente para dicho archivo
        /// y lo abre en una nueva ventana
        /// </summary>
        /// <param name="ruta">Ruta absoluta del archivo que se va a descargar</param>
        /// <param name="nombre">Nombre con el que será descargado el archivo</param>
        public static async Task<Result<bool>> OpenFile(this IJSRuntime jsRuntime, string ruta, string nombre)
        {
            if (!File.Exists(ruta)) return Result<bool>.Failure("La ruta especificada no existe");

            try
            {
                FileInfo fi = new FileInfo(ruta);
                string rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");
                Directory.CreateDirectory(rutaDestino);

                string rutaFinal = Path.Combine(rutaDestino, $"TempFile_{Guid.NewGuid().ToString("N")}{Path.GetExtension(ruta)}");

                if (!ruta.Equals(rutaFinal))
                    File.Copy(ruta, rutaFinal);

                string rutaCliente = rutaFinal.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "").Replace("\\", "/");

                //await jsRuntime.InvokeVoidAsync("downloadURI", ruta_cliente, nombre + fi.Extension);

                await jsRuntime.InvokeVoidAsync("open", rutaCliente, "_blank");

                string usuario = "";

                try
                {
                    usuario = await jsRuntime.InvokeAsync<string>("getActualUser");
                }
                catch (Exception exe)
                {

                }

                AuditoriaDescargaArchivo auditoria = new AuditoriaDescargaArchivo()
                {
                    ExtensionArchivo = fi.Extension,
                    IpAddress = await jsRuntime.InvokeAsync<string>("getIpAddress")
                    .ConfigureAwait(true),
                    NombreArchivo = nombre + fi.Extension,
                    RutaDescargada = rutaCliente,
                    RutaOriginal = ruta,
                    Usuario = usuario,
                    PesoArchivo = new FileInfo(ruta).Length
                };

                await _auditoriaService.RegistrarAuditoriaDescargaArchivo(auditoria);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al abrir archivo");
                return Result<bool>.Failure("Ocurrió un error al procesar el archivo");
            }

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Obtiene la dirección IP del cliente
        /// </summary>
        /// <returns>Dirección IP del cliente</returns>
        public static async Task<string> GetIpAddress(this IJSRuntime jsRuntime)
        {
            try
            {
                var ipAddress = await jsRuntime.InvokeAsync<string>("getIpAddress")
                    .ConfigureAwait(true);
                return ipAddress;
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al obtener ipaddress");
            }

            return string.Empty;
        }
        /// <summary>
        /// Inicializa un timer de inactividad mediante javascript ayudado por una referencia <see cref="DotNetObjectReference{TValue}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dotNetObjectReference">Referencia DotNet </param>
        public static async Task InitializeInactivityTimer<T>(this IJSRuntime jsRuntime,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("initializeInactivityTimer", dotNetObjectReference, 10, 7);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Inicializa una referencia <see cref="DotNetObjectReference{TValue}"/> para realizar un proceso de firma electrónica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dotNetObjectReference">Referencia DotNet </param>
        public static async Task InitializeDotnetHelperFirma<T>(this IJSRuntime jsRuntime,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("initializeDotNetHelperFirma", dotNetObjectReference);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }


        public static async Task LoadJSScript(this IJSRuntime jsRuntime, string url)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("loadScript", url);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, $"Error al cargar script {url}");
            }
        }

        public static async Task DisplaySuccessToastr(this IJSRuntime jsRuntime, string mensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("successToastr", mensaje);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar success toastr");
            }
        }

        public static async Task DisplayWarningToastr(this IJSRuntime jsRuntime, string mensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("warningToastr", mensaje);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar warning toastr");
            }
        }

        public static async Task DisplayErrorToastr(this IJSRuntime jsRuntime, string mensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("errorToastr", mensaje);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar error toastr");
            }
        }

        /// <summary>
        /// Tipos de alertas disponibles para la librería <c>Sweet Alert</c>
        /// </summary>
        public enum TipoMensajeSweetAlert
        {
            question, warning, error, success, info
        }

        public static async Task OpenModalDialog(this IJSRuntime jsRuntime, string idModal, bool fixedBody = true)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("openModalDialog", $"#{idModal}");

                if (fixedBody)
                {
                    await jsRuntime.InvokeVoidAsync("setFixedBody");
                }
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
        }

        public static async Task CloseModalDialog(this IJSRuntime jsRuntime, string idModal, bool fixedBody = true)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("closeModalDialog", $"#{idModal}");

                if (fixedBody)
                {
                    await jsRuntime.InvokeVoidAsync("unsetFixedBody");
                }
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
        }
    }
}
