using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.DTO.Firmas
{
    /// <summary>
    /// Objeto DTO utilizado en los procesos de firma electrónica
    /// </summary>
    public class FirmaElectronicaDTO
    {
        /// <summary>
        /// Ruta del archivo que será firmado
        /// </summary>
        public string RutaOriginal { get; set; } = string.Empty;
        /// <summary>
        /// Ruta final en la que se guardará el archivo ya firmado (final)
        /// </summary>
        public string RutaFinal { get; set; } = string.Empty;
        /// <summary>
        /// Estado de la firma
        /// </summary>
        public bool EstadoFirma { get; set; } = false;
    }
}
