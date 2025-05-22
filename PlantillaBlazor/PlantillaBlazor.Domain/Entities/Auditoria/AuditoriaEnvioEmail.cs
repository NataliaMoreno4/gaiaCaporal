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
    /// Modelo que representa un registro de auditoría de un email enviado
    /// </summary>
    [Table("AuditoriaEnvioEmail", Schema = "Aud")]
    public class AuditoriaEnvioEmail : BaseEntity
    {
        /// <summary>
        /// Destinatarios del email
        /// </summary>
        public string EmailDestinatario { get; set; } = string.Empty;
        /// <summary>
        /// Destinatarios en copia del email
        /// </summary>
        public string EmailCC { get; set; } = string.Empty;
        /// <summary>
        /// Destinatarios en copia oculta del email
        /// </summary>
        public string EmailBCC { get; set; } = string.Empty;
        /// <summary>
        /// Asunto con el que se envió el email
        /// </summary>
        [MaxLength(100)]
        public string Asunto { get; set; } = string.Empty;
        /// <summary>
        /// Bandeja desde la cual se envía el correo
        /// </summary>
        public string EmailEmisor { get; set; } = string.Empty;
        /// <summary>
        /// Contenido del mensaje del email en html
        /// </summary>
        public string MensajeHTML { get; set; } = string.Empty;
        /// <summary>
        /// Indica si el correo fue enviado o no
        /// </summary>
        public bool FueEnviado { get; set; }
        /// <summary>
        /// En caso de no haber sido enviado el email, indica el error generado
        /// </summary>
        public string DescripcionError { get; set; } = string.Empty;
        /// <summary>
        /// Pantalla desde la cual se dispara el correo
        /// </summary>
        [MaxLength(100)]
        public string Pantalla { get; set; } = string.Empty;
        /// <summary>
        /// Concepto bajo el cual se envió el email
        /// </summary>
        public string Concepto { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del proceso para el cual se generó el email
        /// </summary>
        public string NumeroIdentificacionProceso { get; set; } = string.Empty;
        /// <summary>
        /// Lista de adjuntos que fueron enviados en el email
        /// </summary>
        public virtual List<AuditoriaAdjuntoEmail> ListaAdjuntosEmail { get; set; } = new List<AuditoriaAdjuntoEmail>();

        public string? Host { get; set; }
        public int? Puerto { get; set; }
        public bool? SslEnabled { get; set; }
    }
}
