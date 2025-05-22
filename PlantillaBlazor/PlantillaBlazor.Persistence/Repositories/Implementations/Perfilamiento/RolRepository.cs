using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Perfilamiento
{
    public class RolRepository : EFCoreRepository<Rol>, IRolRepository
    {
        public RolRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

        public async Task<Rol> GetRolById(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return context.Roles.Find(id);
        }
        public async Task<bool> InsertarRol(Rol rol)
        {
            using var context = _dbContextFactory.CreateDbContext();

            foreach (var a in rol.ListaRolModulo)
            {
                a.FechaAdicion = DateTime.Now;
            }

            Rol temp = context.Roles
                .Include(r => r.ListaRolModulo)
                .Where(r => r.Id == rol.Id)
                .AsNoTracking()
                .FirstOrDefault();

            if (temp is null)
            {
                rol.FechaAdicion = DateTime.Now;
                context.Roles.Add(rol);
            }
            else
            {
                foreach (var a in rol.ListaRolModulo)
                {
                    a.Id = 0;
                }

                await LimpiarAsignacionModulos(temp.ListaRolModulo);
                rol.FechaUltimaActualizacion = DateTime.Now;
                context.Roles.Update(rol);
            }

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        private async Task LimpiarAsignacionModulos(IEnumerable<RolModulo> rolesModulos)
        {
            using var context = _dbContextFactory.CreateDbContext();

            context.RolModulo.RemoveRange(rolesModulos);

            int entities = await context.SaveChangesAsync();
        }
    }
}
