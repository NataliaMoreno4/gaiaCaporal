using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Parametrizacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Parametrizacion
{
    public class ParametroGeneralRepository : EFCoreRepository<ParametroGeneral>, IParametroGeneralRepository
    {
        public ParametroGeneralRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

        public async Task<bool> InsertarParametroGeneral(ParametroGeneral parametroGeneral)
        {
            parametroGeneral.FechaAdicion = DateTime.Now;

            parametroGeneral.ListaParametrosDetallados.ForEach(p => p.FechaAdicion = DateTime.Now);

            using var context = _dbContextFactory.CreateDbContext();

            foreach (var pDetallado in parametroGeneral.ListaParametrosDetallados)
            {
                var tempP = context.ParametrosDetallados
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == pDetallado.Id);

                if (tempP is null)
                {
                    pDetallado.FechaAdicion = DateTime.Now;
                    pDetallado.IdUsuarioAdiciono = parametroGeneral.IdUsuarioAdiciono;
                }
                else
                {
                    pDetallado.FechaUltimaActualizacion = DateTime.Now;
                    pDetallado.IdUsuarioUltimaActualizacion = parametroGeneral.IdUsuarioUltimaActualizacion;
                }
            }

            context.ParametrosGenerales.Add(parametroGeneral);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
        public async Task<bool> ActualizarParametroGeneral(ParametroGeneral parametroGeneral)
        {
            parametroGeneral.FechaUltimaActualizacion = DateTime.Now;

            using var context = _dbContextFactory.CreateDbContext();

            foreach (var pDetallado in parametroGeneral.ListaParametrosDetallados)
            {
                var tempP = context.ParametrosDetallados
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == pDetallado.Id);

                if (tempP is null)
                {
                    pDetallado.FechaAdicion = DateTime.Now;
                    pDetallado.IdUsuarioAdiciono = parametroGeneral.IdUsuarioAdiciono;
                }
                else
                {
                    pDetallado.FechaUltimaActualizacion = DateTime.Now;
                    pDetallado.IdUsuarioUltimaActualizacion = parametroGeneral.IdUsuarioUltimaActualizacion;
                }
            }

            context.ParametrosGenerales.Update(parametroGeneral);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<ParametroGeneral> ConsultarParametroGeneralById(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            return await context.ParametrosGenerales.FindAsync(id);
        }

        public async Task<bool> EliminarParametroGeneral(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            ParametroGeneral pGeneral = await context.ParametrosGenerales
                .Where(p => p.Id == id)
                .Include(p => p.ListaParametrosDetallados)
                .FirstOrDefaultAsync();

            if (pGeneral is null)
            {
                return false;
            }

            context.ParametrosDetallados.RemoveRange(pGeneral.ListaParametrosDetallados);
            context.ParametrosGenerales.Remove(pGeneral);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
