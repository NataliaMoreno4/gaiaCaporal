
using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using System.Linq.Expressions;

namespace PlantillaBlazor.Services.Implementations.Perfilamiento
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<Session> GetSessionById(long idSession)
        {
            return await _sessionRepository.GetById(idSession);
        }

        public async Task<IEnumerable<Session>> GetSessionsByUsuario(long idUsuario)
        {
            var filtros = new List<Expression<Func<Session, bool>>>();
            filtros.Add(s => s.IdUsuario == idUsuario);

            return await _sessionRepository.Get(filtros: filtros);
        }

        public async Task<Result<bool>> InhabilitarSession(long idSession)
        {
            var session = await _sessionRepository.GetById(idSession);

            if (session is null) return Result<bool>.Failure("Sesión no válida");

            session.IsActive = false;

            if (await _sessionRepository.SaveSessionInfo(session))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("No es posible actualizar la información de la sesión");
        }

        public async Task<Result<long>> ProcesarIngresoUsuario(Session session)
        {
            if
            (
                session.IdUsuario <= 0 ||
                string.IsNullOrEmpty(session.Host) ||
                session.IdAuditoriaLogin is null ||
                string.IsNullOrEmpty(session.IpAddress)
            )
            {
                return Result<long>.Failure("No es posible procesar la sesión");
            }

            if (await _sessionRepository.SaveSessionInfo(session))
            {
                return Result<long>.Success(session.Id);
            }

            return Result<long>.Failure("No es posible guardar la información de la sesión");
        }

        public async Task<Result<bool>> InhabilitarSessionsUsuario(long idUsuario, string motivo)
        {
            if (await _sessionRepository.DesactivarSessionsUsuario(idUsuario, motivo))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("No es posible guardar la información");
        }

        public async Task InhabilitarSesionesInactivas(int dias)
        {
            await _sessionRepository.InhabilitarSesionesInactivas(dias);
        }

        public async Task<bool> InhabilitarSessionByIdAuditoria(long idAuditoria, string motivo)
        {
            return await _sessionRepository.InhabilitarSessionByIdAuditoria(idAuditoria, motivo);
        }

    }
}
