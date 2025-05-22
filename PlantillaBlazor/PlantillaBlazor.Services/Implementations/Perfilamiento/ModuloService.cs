using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Implementations.Perfilamiento
{
    public class ModuloService : IModuloService
    {
        private readonly IModuloRepository _moduloRepository;

        public ModuloService(IModuloRepository moduloRepository)
        {
            _moduloRepository = moduloRepository;
        }

        public async Task<bool> EliminarAsignacionesRolModulo(IEnumerable<RolModulo> asignaciones)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Modulo>> GetModulos()
        {
            return await _moduloRepository.Get();
        }

        public IEnumerable<Modulo> GetModulosSync()
        {
            return _moduloRepository.GetSync();
        }

        public async Task<bool> InsertarAsignacionesRolModulo(IEnumerable<RolModulo> asignaciones)
        {
            throw new NotImplementedException();
        }
    }
}
