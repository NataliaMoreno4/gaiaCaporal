using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using PlantillaBlazor.Web.Services.Circuits;
using PlantillaBlazor.Web.Entities.Authorization;

namespace PlantillaBlazor.Web.Services.Authentication
{
    public class AuthService
    {
        private readonly ICircuitUserService _circuitUserServer;
        private readonly CircuitHandler _blazorCircuitHandler;

        private readonly ILogger<AuthService> _logger;

        public AuthService
        (
            ICircuitUserService circuitUserServer,
            CircuitHandler blazorCircuitHandler,
            AuthenticationStateProvider authStateProvider,

            ILogger<AuthService> logger,
            NavigationManager navigationManager
        )
        {
            _circuitUserServer = circuitUserServer;
            _blazorCircuitHandler = blazorCircuitHandler;
            _logger = logger;
        }

        public void conectarCircuito(UserSession session)
        {
            CircuitHandlerService handler = (CircuitHandlerService)_blazorCircuitHandler;
            _circuitUserServer.Connect(handler.CirtuidId, session);
        }

        public void desconectarCircuito()
        {
            CircuitHandlerService handler = (CircuitHandlerService)_blazorCircuitHandler;
            _circuitUserServer.Disconnect(handler.CirtuidId);
        }
    }
}
