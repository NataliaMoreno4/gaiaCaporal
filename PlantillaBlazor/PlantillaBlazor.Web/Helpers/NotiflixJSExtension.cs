using Microsoft.JSInterop;
using Serilog;

namespace PlantillaBlazor.Web.Helpers
{
    public static class NotiflixJSExtension
    {
        public static async Task NotiflixNotifyAlert(this IJSRuntime jsRuntime, string message, NotiflixMessageType messageType)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("NotiflixNotifyAlert", message, messageType.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar notify alert notiflix");
            }
        }

        public static async Task NotiflixReportAlert(this IJSRuntime jsRuntime, string title, string message, NotiflixMessageType messageType, string buttonText = "Aceptar", string width = "360px", string svgSize = "120px")
        {
            try
            {
                await jsRuntime.NotiflixRemoveLoading();
                await jsRuntime.InvokeVoidAsync("NotiflixReportAlert", title, message, buttonText, width, svgSize, messageType.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar notify report notiflix");
            }
        }

        public static async Task NotiflixReportAlertWithCallback(this IJSRuntime jsRuntime, string title, string message, NotiflixMessageType messageType, string buttonText = "Aceptar", string width = "360px", string svgSize = "120px")
        {
            try
            {
                await jsRuntime.NotiflixRemoveLoading();
                await jsRuntime.InvokeVoidAsync("NotiflixReportAlertCallback", title, message, buttonText, width, svgSize, messageType.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar report alert notiflix con callback");
            }
        }

        public static async Task<bool> NotiflixConfirmShow(this IJSRuntime jsRuntime, string title, string message, string okButtonText = "Aceptar", string cancelButtonText = "Cancelar", string width = "320px")
        {
            try
            {
                return await jsRuntime.InvokeAsync<bool>("NotiflixConfirmShow", title, message, okButtonText, cancelButtonText, width);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar confirm show alert notiflix");
            }

            return false;
        }

        public static async Task<NotiflixPrompResponse> NotiflixConfirmPrompt(this IJSRuntime jsRuntime, string title, string question, string okButtonText = "Aceptar", string cancelButtonText = "Cancelar", string defaultAnswer = "", string width = "320px")
        {
            try
            {
                return await jsRuntime.InvokeAsync<NotiflixPrompResponse>("NotiflixConfirmPrompt", title, question, okButtonText, cancelButtonText, defaultAnswer, width);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar confirm prompt alert notiflix");
            }

            return null!;
        }

        public static async Task NotiflixLoading(this IJSRuntime jsRuntime, NotiflixLoadingType loadingIcon, string text = "", string svgSize = "19px")
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("NotiflixLoading", text, loadingIcon.ToString(), svgSize);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar loading notiflix");
            }
        }

        public static async Task NotiflixRemoveLoading(this IJSRuntime jsRuntime)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("NotiflixRemoveLoading");
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al remover loading notiflix");
            }
        }

        public static async Task NotiflixBlock(this IJSRuntime jsRuntime, NotiflixLoadingType loadingIcon, string selector, string text = "")
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("NotiflixBlock", selector, text, loadingIcon.ToString());
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al mostrar block loading notiflix");
            }
        }

        public static async Task NotiflixRemoveBlock(this IJSRuntime jsRuntime, string selector)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("NotiflixRemoveBlock", selector);
            }
            catch (TaskCanceledException tce)
            {

            }
            catch (JSDisconnectedException jde)
            {

            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al remover block loading notiflix");
            }
        }
        public enum NotiflixLoadingType
        {
            standard,
            hourglass,
            circle,
            arrows,
            dots,
            pulse
        }

        public enum NotiflixMessageType
        {
            success,
            failure,
            warning,
            info
        }

        public class NotiflixPrompResponse
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
