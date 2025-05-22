using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Common;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        public Task<bool> RegistrarAuditoriaLogin(AuditoriaLoginUsuario auditoria);
        public Task<bool> RegistrarAuditoriaCierreSesion(AuditoriaLoginUsuario auditoria);
        public Task<bool> InsertarUsuario(Usuario usuario);
        public Task<long> GetCantidadIntentosFallidos(long idUsuario, int cantidadMinutos);

        /// <summary>
        /// Inactiva aquellos usuarios que no han entrado a la plataforma después de n cantidad de días
        /// </summary>
        /// <param name="diasDesdeUltimoLoggeo">Días desde los cuales no se han loggeado los usuarios</param>
        /// <returns>Booleano indicando si la operación fue exitosa</returns>
        public Task<bool> InactivarUsuariosNoActivos(int diasDesdeUltimoLoggeo);
        public Task<AuditoriaLoginUsuario> GetAuditoriaLoginUsuario(long id);
    }
}
