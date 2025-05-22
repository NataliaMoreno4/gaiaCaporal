namespace PlantillaBlazor.Domain.DTO.Firmas
{
    public class EspacioFirmaDTO
    {
        /// <summary>
        /// Identificador de la firma, generada por base de datos
        /// </summary>
        public long id_firma_firmante { get; set; }
        /// <summary>
        /// Identificador detalle firmante-documento al cual está ligada la firma
        /// </summary>
        public long id_detalle_firmante_documento { get; set; }
        /// <summary>
        /// Página dentro del documento en la cuál estará ubicada la firma
        /// </summary>
        public string pagina { get; set; } = "";
        /// <summary>
        /// Representa el alto de la firma dentro del documento
        /// </summary>
        public string height { get; set; } = "";
        /// <summary>
        /// Representa el ancho de la firma dentro del documento
        /// </summary>
        public string width { get; set; } = "";
        /// <summary>
        /// Representa la posición x de la firma dentro del documento
        /// </summary>
        public string x { get; set; } = "";
        /// <summary>
        /// Representa la posición y de la firma dentro del documento
        /// </summary>
        public string y { get; set; } = "";
        /// <summary>
        /// Fecha de creación del registro de firma
        /// </summary>
        public DateTime fecha_adicion { get; set; }

        public string tipo { get; set; } = "";
        public string font_size { get; set; } = "";
        public string line_height { get; set; } = "";
        public string contenido { get; set; } = "";
    }
}
