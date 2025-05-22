using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFUtilities
{
    /// <summary>
    /// Representa la información de una firma realizada sobre un documento, entre las cuales está:
    /// pósición x-y, ancho, alto, página dentro del documento, etc.
    /// </summary>
    public class SignElement
    {
        /// <summary>
        /// Página dentro del documento en la cuál estará ubicada la firma
        /// </summary>
        public int NumeroPagina { get; set; }
        /// <summary>
        /// Representa el alto de la firma dentro del documento
        /// </summary>
        public string Height { get; set; } = string.Empty;
        /// <summary>
        /// Representa el ancho de la firma dentro del documento
        /// </summary>
        public string Width { get; set; } = string.Empty;
        /// <summary>
        /// Representa la posición x de la firma dentro del documento
        /// </summary>
        public string CoordenadaX { get; set; } = string.Empty;
        /// <summary>
        /// Representa la posición y de la firma dentro del documento
        /// </summary>
        public string CoordenadaY { get; set; } = string.Empty;
        /// <summary>
        /// Tipo de elemento de firma: Espacio de firma, firma, texto, checks, radios, etc
        /// </summary>
        public string Tipo { get; set; } = "";
        /// <summary>
        /// Tamaño de fuente en caso de que el elemento de firma sea un texto
        /// </summary>
        public string FontSize { get; set; } = string.Empty;
        /// <summary>
        /// Alto de línea en caso de que el elemento de firma sea un texto
        /// </summary>
        public string LineHeight { get; set; } = string.Empty;
        /// <summary>
        /// Contenido del elemento de firma, puede ser texto o el contenido de la imagen en base64
        /// </summary>
        public string Contenido { get; set; } = "";
    }
}
