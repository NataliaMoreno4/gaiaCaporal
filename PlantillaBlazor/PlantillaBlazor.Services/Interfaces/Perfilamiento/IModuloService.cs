using PlantillaBlazor.Domain.Entities.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Perfilamiento
{
    public interface IModuloService
    {
        public Task<IEnumerable<Modulo>> GetModulos();
        public IEnumerable<Modulo> GetModulosSync();
        public Task<bool> EliminarAsignacionesRolModulo(IEnumerable<RolModulo> asignaciones);
        public Task<bool> InsertarAsignacionesRolModulo(IEnumerable<RolModulo> asignaciones);
    }
}
