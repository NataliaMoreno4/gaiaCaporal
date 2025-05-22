using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Registraduria;
using Microsoft.EntityFrameworkCore;


namespace PlantillaBlazor.Persistence.Repositories.Implementations.Registraduria
{
    public class RegistraduriaRepository : EFCoreRepository<AuditoriaConsumoRegistraduria>, IRegistraduriaRepository
    {
        public RegistraduriaRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<AuditoriaConsumoRegistraduria> ConsultarRegistroRegistraduria(string cedula)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var consulta = await context.AuditoriaConsumoRegistraduria
                .Where(a => a.CedulaConsultada.Equals(cedula))
                .Where(a => a.StatusCodeRespuesta.Equals("OK"))
                .Where(a => string.IsNullOrEmpty(a.Error))
                .Where(a => EF.Functions.DateDiffDay(a.FechaFinConsulta, DateTime.Now) <= 30)
                .FirstOrDefaultAsync();

            return consulta;
        }
    }
}
