using System.Security.Claims;

namespace PlantillaBlazor.Web.Entities.Authorization
{
    /// <summary>
    /// Representa el modelo mediante el cual se describe una sesión de usuario dentro del sistema
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public long IdUsuario { get; set; }
        /// <summary>
        /// Indica si el usuario marcó o no que desea que su usuario sea recordado por el sistema para futuros ingresos
        /// </summary>
        public bool RememberMe { get; set; } = false;
        /// <summary>
        /// Dirección de ip con la cual ingresa el usuario
        /// </summary>
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// Fecha del último login exitoso para el usuario
        /// </summary>
        public DateTime? LastLogin { get; set; }
        /// <summary>
        /// Identificador del registro de auditoría asociado al último login del usuario
        /// </summary>
        public string IdAuditoriaLogin { get; set; } = string.Empty;
        /// <summary>
        /// Url desde la cual fue generada la sesión de usuario
        /// </summary>
        public string Url { get; set; } = string.Empty;
        public string IdSession { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;

        public static UserSession GetUserSessionFromClaims(IEnumerable<Claim> claims)
        {
            UserSession userSession = new();

            var idUsuario = claims.FirstOrDefault(c => c.Type == "IdUsuario")!.Value;
            var idSession = claims.FirstOrDefault(c => c.Type == "IdSession")!.Value;
            var idAuditoria = claims.FirstOrDefault(c => c.Type == "IdAuditoria")!.Value;
            var rememberMe = claims.FirstOrDefault(c => c.Type == "RememberMe")!.Value;
            var url = claims.FirstOrDefault(c => c.Type == "Url")!.Value;
            var tipoUsuario = claims.FirstOrDefault(c => c.Type == "TipoUsuario")!.Value;

            userSession.IdUsuario = long.Parse(idUsuario);
            userSession.IdAuditoriaLogin = idAuditoria;
            userSession.RememberMe = bool.Parse(rememberMe);
            userSession.IdSession = idSession;
            userSession.Url = url;
            userSession.TipoUsuario = tipoUsuario;

            return userSession;
        }

    }
}
