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
    /// Modelo que representa un registro de auditoría de un mensaje SMS
    /// </summary>
    [Table("AuditoriaEnvioSMS", Schema = "Aud")]
    public class AuditoriaEnvioSMS : BaseEntity
    {
        /// <summary>
        /// Celular al cual fue enviado el mensaje
        /// </summary>
        [MaxLength(30)]
        public string CelularDestinatario { get; set; } = string.Empty;
        /// <summary>
        /// Contenido del mensaje enviado
        /// </summary>
        [MaxLength(200)]
        public string Mensaje { get; set; } = string.Empty;
        /// <summary>
        /// Fecha en la que se envió el mensaje
        /// </summary>
        public DateTime FechaEnvio { get; set; }
        /// <summary>
        /// Indica si el mensaje fue enviado
        /// </summary>
        public bool FueEnviado { get; set; }
        /// <summary>
        /// Identificador del proceso para el cual fue generado el mensaje
        /// </summary>
        [MaxLength(200)]
        public string IdentificacionProceso { get; set; } = string.Empty;
        /// <summary>
        /// Concepto del proceso para el cual fue generado el mensaje
        /// </summary>
        [MaxLength(200)]
        public string Concepto { get; set; } = string.Empty;
        /// <summary>
        /// Pantalla desde la cual se genera el mensaje
        /// </summary>
        [MaxLength(200)]
        public string Pantalla { get; set; } = string.Empty;

        public string? StatusCode { get; set; }
        public string? Error { get; set; }
        public string? ContenidoRespuesta { get; set; }
        public string? ContenidoBody { get; set; }
        public string? UrlRequest { get; set; }
    }
}
