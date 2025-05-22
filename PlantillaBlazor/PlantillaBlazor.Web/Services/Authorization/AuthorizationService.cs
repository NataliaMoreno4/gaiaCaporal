using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace PlantillaBlazor.Web.Services.Authorization
{
    /// <summary>
    /// Servicio encargado de llevar la autorización de cada módulo del sistema para cada uno de los roles de éste
    /// </summary>
    public class AuthorizationService : IDisposable
    {
        public readonly IEnumerable<Modulo> modulos;
        public readonly IEnumerable<Rol> roles;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public AuthorizationService(
            IModuloService moduloService,
            IRolService rolService,
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            modulos = moduloService.GetModulosSync();
            if (modulos is null) modulos = new List<Modulo>();

            roles = rolService.GetRolesSync();
            if (roles is null) roles = new List<Rol>();

            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;


            _navigationManager.LocationChanged += InfoCliente;
        }

        /// <summary>
        /// Evento que se disparará cada vez que se detecte un cambio de localización/url dentro del aplicativo. Dispara una función que invoca una función de javascript que obtendrá todos los datos del cliente como ubicación, useragent, versión del navegador, entre otros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InfoCliente(object sender, LocationChangedEventArgs e)
        {
            await _jsRuntime.InvokeVoidAsync("getClientInfo");
        }

        void IDisposable.Dispose()
        {
            // Unsubscribe from the event when our component is disposed
            _navigationManager.LocationChanged -= InfoCliente;
        }
    }


}
