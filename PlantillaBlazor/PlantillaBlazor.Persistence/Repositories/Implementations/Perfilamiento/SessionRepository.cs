
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using Microsoft.EntityFrameworkCore;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.Perfilamiento
{
    public class SessionRepository : EFCoreRepository<Session>, ISessionRepository
    {
        public SessionRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<bool> DesactivarSessionsUsuario(long idUsuario, string motivo)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var entities = await context.Sessions
                .AsNoTracking()
                .Where(s => s.IdUsuario == idUsuario)
                .Where(s => s.IsActive == true)
                .ExecuteUpdateAsync(
                    s =>
                        s
                        .SetProperty(se => se.IsActive, se => false)
                        .SetProperty(se => se.Observaciones, se => motivo)
                        .SetProperty(se => se.FechaInactivacion, se => DateTime.Now)
                );

            return entities > 0;

        }

        public async Task InhabilitarSesionesInactivas(int dias)
        {
            using var context = _dbContextFactory.CreateDbContext();

            await context.Sessions
                .AsNoTracking()
                .Where(s => EF.Functions.DateDiffDay(s.FechaUltimoIngreso!.Value.Date, DateTime.Now.Date) > dias)
                .Where(s => s.IsActive == true)
                .ExecuteUpdateAsync(
                    s =>
                        s
                        .SetProperty(se => se.IsActive, se => false)
                        .SetProperty(se => se.Observaciones, se => "Se desactiva por política de último ingreso")
                        .SetProperty(se => se.FechaInactivacion, se => DateTime.Now)
                );

            await context.Sessions
                .AsNoTracking()
                .Where(s => EF.Functions.DateDiffDay(s.FechaUltimoIngreso!.Value.Date, DateTime.Now.Date) > 30)
                .Where(s => s.IsActive == false)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> SaveSessionInfo(Session session)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var tempSession = context.Sessions
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == session.Id);

            session.FechaUltimoIngreso = DateTime.Now;

            if (tempSession is null)
            {
                session.IsActive = true;
                session.FechaAdicion = DateTime.Now;
                context.Sessions.Add(session);
            }
            else
            {
                session.FechaUltimaActualizacion = DateTime.Now;
                session.FechaUltimoIngreso = DateTime.Now;
                context.Sessions.Update(session);
            }

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }

        public async Task<bool> InhabilitarSessionByIdAuditoria(long idAuditoria, string motivo)
        {
            using var context = _dbContextFactory.CreateDbContext();

            int entities = await context.Sessions
                .AsNoTracking()
                .Where(s => s.IdAuditoriaLogin == idAuditoria)
                .ExecuteUpdateAsync(
                    s =>
                        s
                        .SetProperty(se => se.IsActive, se => false)
                        .SetProperty(se => se.Observaciones, se => motivo)
                        .SetProperty(se => se.FechaInactivacion, se => DateTime.Now)
                );

            return entities > 0;
        }

    }
}
