using PlantillaBlazor.Domain.Entities.Auditoria;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.Perfilamiento
{
    /// <summary>
    /// Modelo que representa la información de un usuario
    /// </summary>
    [Table("Usuario", Schema = "Seg")]
    public class Usuario : BaseEntity
    {
        /// <summary>
        /// Nombres del usuario
        /// </summary>
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;
        /// <summary>
        /// Apellidos del usuario
        /// </summary>
        [MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;
        /// <summary>
        /// Nombre de usuario del usuario
        /// </summary>
        [MaxLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;
        /// <summary>
        /// Hash de la contraseña del usuario
        /// </summary>
        public string Clave { get; set; } = string.Empty;
        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del rol que tiene asignado el usuario
        /// </summary>
        public long IdRol { get; set; }
        /// <summary>
        /// Referencia al rol asignado al usuario
        /// </summary>
        public virtual Rol Rol { get; set; }
        /// <summary>
        /// Indica si el usuario está activo o no
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Fecha del último cambio de contraseña realizado por el usuario
        /// </summary>
        public DateTime? FechaUltimoCambioContraseña { get; set; }
        /// <summary>
        /// Indica si el usuario debe cambiar su contraseña en el próximo ingreso al aplicativo
        /// </summary>
        public bool MustChangePassword { get; set; } = false;
        /// <summary>
        /// Celular del usuario
        /// </summary>
        [MaxLength(200)]
        public string Celular { get; set; } = string.Empty;
        /// <summary>
        /// Indica si el usuario tiene activado no el 2FA mediante TOTP
        /// </summary>
        public bool IsTwoFAEnabled { get; set; } = false;

        /// <summary>
        /// Indica la última fecha en la que el usuario ingresó a la plataforma
        /// </summary>
        public DateTime? FechaUltimoIngreso { get; set; }
        [MaxLength(50)]
        public string? TipoUsuario { get; set; } = "Normal";

        /// <summary>
        /// Lista de solicitudes de recuperación de contraseña solicitadas por el usuario
        /// </summary>
        public virtual List<SolicitudRecuperacionClave> ListaSolicitudesRecuperacionClaves { get; set; } = new List<SolicitudRecuperacionClave>();
        /// <summary>
        /// Lista de registros de auditoria de login del usuario
        /// </summary>
        public virtual List<AuditoriaLoginUsuario> ListaAuditoriasLoginUsuario { get; set; } = new List<AuditoriaLoginUsuario>();
        /// <summary>
        /// Lista de registros de auditoría de eventos asociados al usuario
        /// </summary>
        public virtual List<AuditoriaEvento> ListaAuditoriasEventos { get; set; } = new List<AuditoriaEvento>();

        public virtual List<Session> Sessiones { get; set; } = new();
    }
}
