
using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.Perfilamiento;

namespace PlantillaBlazor.Services.Interfaces.Perfilamiento
{
    public interface ISessionService
    {
        public Task<Result<long>> ProcesarIngresoUsuario(Session session);
        public Task<Result<bool>> InhabilitarSessionsUsuario(long idUsuario, string motivo);
        public Task<Result<bool>> InhabilitarSession(long idSession);
        public Task<Session> GetSessionById(long idSession);
        public Task<IEnumerable<Session>> GetSessionsByUsuario(long idUsuario);
        public Task InhabilitarSesionesInactivas(int dias);
        public Task<bool> InhabilitarSessionByIdAuditoria(long idAuditoria, string motivo);
    }
}
