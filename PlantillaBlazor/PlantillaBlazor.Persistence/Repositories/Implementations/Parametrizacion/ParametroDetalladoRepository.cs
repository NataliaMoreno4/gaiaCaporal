using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Parametrizacion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Parametrizacion
{
    public class ParametroDetalladoRepository : EFCoreRepository<ParametroDetallado>, IParametroDetalladoRepository
    {
        public ParametroDetalladoRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

        public async Task<bool> EliminarParametroDetallado(long id)
        {
            using var context = _dbContextFactory.CreateDbContext();

            ParametroDetallado pDetallado = await context.ParametrosDetallados.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (pDetallado is null)
            {
                return false;
            }

            context.ParametrosDetallados.Remove(pDetallado);

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
