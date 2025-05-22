using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Email
{
    /// <summary>
    /// Representa la información de un adjunto que será envíado en un correo electrónico
    /// </summary>
    public class AdjuntoEmailDTO
    {
        /// <summary>
        /// Ruta absoluta del archivo que será añadido como anexo
        /// </summary>
        public string RutaAdjunto { get; set; } = "";
        /// <summary>
        /// Nombre con el que el anexo llegará a todos los destinatarios del e-mail
        /// </summary>
        public string NombreArchivo { get; set; } = "";
    }
}
