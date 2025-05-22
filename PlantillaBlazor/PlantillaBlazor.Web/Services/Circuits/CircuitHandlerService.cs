using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http.Features;

namespace PlantillaBlazor.Web.Services.Circuits
{
    public class CircuitHandlerService : CircuitHandler
    {
        /// <summary>
        /// Identificador del circuito actual
        /// </summary>
        public string CirtuidId { get; private set; }

        ICircuitUserService _userService;

        public CircuitHandlerService(ICircuitUserService userService)
        {
            _userService = userService;
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            CirtuidId = circuit.Id;
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _userService.Disconnect(circuit.Id);
            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }
    }
}
