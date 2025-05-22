using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Email
{
    /// <summary>
    /// Representa el modelo para almacenar la informació necesaria para realizar un envío de correo
    /// </summary>
    public class EmailInfoDTO
    {
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como destinatarios principales
        /// </summary>
        public List<string> Destinatarios { get; set; } = new List<string>();
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como copias
        /// </summary>
        public List<string> Cc { get; set; } = new List<string>();
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como copias ocultas
        /// </summary>
        public List<string> Bcc { get; set; } = new List<string>();

        /// <summary>
        /// Asunto que llevará el email
        /// </summary>
        public required string Asunto { get; set; } = "";
        /// <summary>
        /// Mensaje (<c>HTML</c>) que llevará el e-mail
        /// </summary>
        public required string Mensaje { get; set; } = "";
        /// <summary>
        /// Descripción del envío del correo
        /// </summary>
        public required string Descripcion { get; set; } = "";
        /// <summary>
        /// Usuario que dispara el envío del e-mail
        /// </summary>
        public string Usuario { get; set; } = "";
        /// <summary>
        /// Pantalla desde la cual se genera el envío del e-mail
        /// </summary>
        public string Pantalla { get; set; } = "";
        /// <summary>
        /// Identificador del proceso relacionado
        /// </summary>
        public required string IdentificacionProceso { get; set; } = "";
        /// <summary>
        /// Lista de anexos <see cref="Anexo"/> que serán añadidos en el envío del e-mail
        /// </summary>
        public List<AdjuntoEmailDTO> Adjuntos { get; set; } = new List<AdjuntoEmailDTO>();
    }
}
