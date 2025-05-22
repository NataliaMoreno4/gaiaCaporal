using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    /// <summary>
    /// Modelo que representa la información de un registro de auditoría Otp
    /// </summary>
    [Table("AuditoriaOtp", Schema = "Aud")]
    public class AuditoriaOtp : BaseEntity
    {
        /// <summary>
        /// Código Otp
        /// </summary>
        [MaxLength(30)]
        public string Codigo { get; set; } = string.Empty;
        /// <summary>
        /// Fecha en la que se verificó correctamente el código OTP
        /// </summary>
        public DateTime? FechaValidacion { get; set; }
        /// <summary>
        /// Estado actual del código OTP
        /// </summary>
        [MaxLength(30)]
        public string Estado { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del proceso para el que se generó el código OTP
        /// </summary>
        [MaxLength(200)]
        public string IdentificacionProceso { get; set; } = string.Empty;
        /// <summary>
        /// Tipo de proceso para el que se generó el código OTP
        /// </summary>
        [MaxLength(100)]
        public string TipoProceso { get; set; } = string.Empty;
        /// <summary>
        /// Descripción adicional del código OTP
        /// </summary>
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Determina los métodos mediante los cuales fue enviado el código OTP
        /// </summary>
        [MaxLength(100)]
        public string MetodosEnvio { get; set; } = string.Empty;

    }
}
