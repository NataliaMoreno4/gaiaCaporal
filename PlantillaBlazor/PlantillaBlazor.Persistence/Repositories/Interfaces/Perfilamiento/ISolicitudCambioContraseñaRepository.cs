using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento
{
    public interface ISolicitudCambioContraseñaRepository : IGenericRepository<SolicitudRecuperacionClave>
    {
        public Task<bool> InsertarSolicitudRecuperacionContraseña(SolicitudRecuperacionClave solicitud);

        public Task<bool> InhabilitarSolicitudesCaducadas(int dias);
    }
}
