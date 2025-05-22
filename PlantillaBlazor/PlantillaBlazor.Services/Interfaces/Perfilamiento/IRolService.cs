using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Perfilamiento
{
    public interface IRolService
    {
        public Task<IEnumerable<Rol>> GetRoles();
        public IEnumerable<Rol> GetRolesSync();
        public Task<Rol> GetRol(long idRol);
        public Task<Result<long>> InsertarRol(Rol rol, string ipAddress);
        public Task<IEnumerable<Modulo>> GetModulosRol(long idRol);
        public IEnumerable<Modulo> GetModulosRolSync(long idRol);
        public Result<bool> ValidarInformacionRol(Rol rol);
    }
}
