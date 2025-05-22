using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFUtilities
{
    /// <summary>
    /// Representa el modelo utilizado para describir las propiedades de una firma digital
    /// </summary>
    public class FirmaProperties
    {
        /// <summary>
        /// Ruta del documento a firmar
        /// </summary>
        public required string RutaDocumentoOriginal { get; set; } = "";
        /// <summary>
        /// Ruta en la cual se guardará el documento ya firmado (final)
        /// </summary>
        public required string RutaDocumentoFinal { get; set; } = "";
        /// <summary>
        /// Nombre del firmante que aparecerá en el estampado de la firma
        /// </summary>
        public required string NombreFirmante { get; set; } = "";
        /// <summary>
        /// Nombre del cliente que aparecerá en el estampado de la firma
        /// </summary>
        public required string NombreCliente { get; set; } = "";
        /// <summary>
        /// Número de documento del cliente que aparecerá en el estampado de la firma
        /// </summary>
        public required string DocumentoCliente { get; set; } = "";
        /// <summary>
        /// Coordenada X del PDF en la que se ubicará el estampado de la firma
        /// </summary>
        public required int CoordenadaX { get; set; } = 0;
        /// <summary>
        /// Coordenada Y del PDF en la que se ubicará el estampado de la firma
        /// </summary>
        public required int CoordenadaY { get; set; } = 0;
        /// <summary>
        /// Número de página en la que se estampará la firma
        /// </summary>
        public required int Pagina { get; set; } = 0;
        /// <summary>
        /// Ancho de la firma
        /// </summary>
        public required int Width { get; set; } = 0;
        /// <summary>
        /// Alto de la firma
        /// </summary>
        public required int Height { get; set; } = 0;
        /// <summary>
        /// Objeto <see cref="Certificado"/> el cual describe la información del certificado que será usado para realizar la firma del documento
        /// </summary>
        public required Certificado Certificado { get; set; }
        /// <summary>
        /// Ruta absoluta de la imagen que acompañará a la firma digital
        /// </summary>
        public string RutaImagenFirma { get; set; } = "";
        /// <summary>
        /// Razón que se indicará en la firma_digital
        /// </summary>
        public required string Reason { get; set; } = "";
        /// <summary>
        /// Ubicación geográfica que se indicará en la firma digital
        /// </summary>
        public required string Location { get; set; } = "";

    }
    /// <summary>
    /// Representa el modelo utilizado para describir la información de un certificado de firma en formato PFX
    /// </summary>
    public class Certificado
    {
        /// <summary>
        /// Ruta absoluta del certificado PFX
        /// </summary>
        public required string RutaCertificado { get; set; } = "";
        /// <summary>
        /// Contraseña del certificado PFX
        /// </summary>
        public required string ContraseñaCertificado { get; set; } = "";
    }

    public enum TipoFirmaDigital
    {
        NoImagen,
        ImagenAlLado,
        ImagenCompleta
    }
}
