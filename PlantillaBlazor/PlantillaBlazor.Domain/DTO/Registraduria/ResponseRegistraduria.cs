namespace PlantillaBlazor.Domain.DTO.Registraduria
{
    public class ResponseRegistraduria
    {
        public string Cedula { get; set; } = string.Empty;
        public string CodigoErrorCedula { get; set; } = string.Empty;
        public string EstadoCedula { get; set; } = string.Empty;
        public string DepartamentoExpedicionDocumento { get; set; } = string.Empty;
        public DateTime? FechaExpedicionDocumento { get; set; }
        public string MunicipioExpedicionDocumento { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = string.Empty;
        public string SegundoNombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
    }
}
