using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento
{
    public interface IRolRepository : IGenericRepository<Rol>
    {
        public Task<bool> InsertarRol(Rol rol);
        public Task<Rol> GetRolById(long id);
    }
}
