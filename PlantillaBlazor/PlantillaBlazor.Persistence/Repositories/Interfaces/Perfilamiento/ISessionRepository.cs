
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Common;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        public Task<bool> SaveSessionInfo(Session session);
        public Task<bool> DesactivarSessionsUsuario(long idUsuario, string motivo);
        public Task InhabilitarSesionesInactivas(int dias);
        public Task<bool> InhabilitarSessionByIdAuditoria(long idAuditoria, string motivo);
    }
}
