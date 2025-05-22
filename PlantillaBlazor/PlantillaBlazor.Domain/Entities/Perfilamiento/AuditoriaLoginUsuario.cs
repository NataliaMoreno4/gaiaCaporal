using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Perfilamiento
{
    /// <summary>
    /// Modelo que representa la auditoría de login de usuario
    /// </summary>
    [Table("AuditoriaLoginUsuario", Schema = "Seg")]
    public class AuditoriaLoginUsuario : BaseEntity
    {
        /// <summary>
        /// Identificador del usuario asociado
        /// </summary>
        public long IdUsuario { get; set; }
        /// <summary>
        /// Referencia al usuario asociado
        /// </summary>
        public virtual Usuario Usuario { get; set; }
        /// <summary>
        /// Fecha en la que se realizó el login
        /// </summary>
        public DateTime FechaLogin { get; set; }
        /// <summary>
        /// Descripción del evento de login
        /// </summary>
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Dirección IP desde la cual se realizó el login
        /// </summary>
        [MaxLength(50)]
        public string IpLogin { get; set; } = string.Empty;
        /// <summary>
        /// Fecha en la que se realizó el cierre de sesión
        /// </summary>
        public DateTime? FechaCierreSesion { get; set; }
        /// <summary>
        /// Dirección IP desde la cual se cerró la sesión
        /// </summary>
        [MaxLength(50)]
        public string IpCierreSesion { get; set; } = string.Empty;
        /// <summary>
        /// Motivo por el cual se cerró la sesión
        /// </summary>
        public string MotivoCierreSesion { get; set; } = string.Empty;
    }
}
