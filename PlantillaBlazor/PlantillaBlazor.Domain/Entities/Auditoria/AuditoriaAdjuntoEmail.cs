using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    /// <summary>
    /// Modelo que representa un adjunto enviado dentro de un correo electrónico
    /// </summary>
    [Table("AuditoriaAdjuntoEmail", Schema = "Aud")]
    public class AuditoriaAdjuntoEmail : BaseEntity
    {
        /// <summary>
        /// Ruta absoluta del archivo
        /// </summary>
        public string RutaAbsolutaAdjunto { get; set; } = string.Empty;
        /// <summary>
        /// Nombre con el cual se adjunta el archivo dentro del correo electrónico
        /// </summary>
        public string NombreAdjunto { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del registro de auditoría del email relacionado
        /// </summary>
        public long IdAuditoriaEnvioEmail { get; set; }
        /// <summary>
        /// Referencia del registro de auditoría del email relacionado
        /// </summary>
        public virtual AuditoriaEnvioEmail AuditoriaEnvioEmail { get; set; }
    }
}
