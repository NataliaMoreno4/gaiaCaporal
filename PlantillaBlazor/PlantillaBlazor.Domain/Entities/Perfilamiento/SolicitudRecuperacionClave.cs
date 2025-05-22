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
    /// Modelo que representa la información de una solicitud de recuperación de clave
    /// </summary>
    [Table("SolicitudRecuperacionClave", Schema = "Seg")]
    public class SolicitudRecuperacionClave : BaseEntity
    {
        /// <summary>
        /// Identificador del usuario que realiza la solicitud de recuperación de contraseña
        /// </summary>
        public required long IdUsuario { get; set; }
        /// <summary>
        /// Referencia al usuario que realiza la solicitud de recuperación de contraseña
        /// </summary>
        public virtual Usuario Usuario { get; set; }
        /// <summary>
        /// Estado actual de la solicitud de recuperación de contraseña
        /// </summary>
        [MaxLength(30)]
        public required string Estado { get; set; }
        /// <summary>
        /// Fecha en la que se finalizó la solicitud de recuperación de contraseña
        /// </summary>
        public DateTime? FechaFinalizacion { get; set; }
        /// <summary>
        /// Dirección IP desde la cual se creó la solicitud de recuperación de contraseña
        /// </summary>
        [MaxLength(30)]
        public required string IpAccionInicial { get; set; } = string.Empty;
        /// <summary>
        /// Dirección IP desde la cual se finalizó la solicitud de recuperación de contraseña
        /// </summary>
        [MaxLength(30)]
        public string IpAccionFinal { get; set; } = string.Empty;
        /// <summary>
        /// Motivo por el cual se generó la solicitud de recuperación de contraseña
        /// </summary>
        [MaxLength(200)]
        public required string MotivoCambioContraseña { get; set; } = string.Empty;

    }
}
