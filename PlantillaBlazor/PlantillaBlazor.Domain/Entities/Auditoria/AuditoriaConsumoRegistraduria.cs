using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    [Table("AuditoriaConsumoRegistraduria", Schema = "Aud")]
    public class AuditoriaConsumoRegistraduria : BaseEntity
    {
        public DateTime? FechaInicioConsulta { get; set; }
        public DateTime? FechaFinConsulta { get; set; }
        [MaxLength(100)]
        public string StatusCodeRespuesta { get; set; } = string.Empty;
        public string BodyRequest { get; set; } = string.Empty;
        public string JsonResponse { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        [MaxLength(200)]
        public string CedulaConsultada { get; set; } = string.Empty;
        [MaxLength(200)]
        public string IpConsulta { get; set; } = string.Empty;
        [MaxLength(200)]
        public string UsuarioConsulta { get; set; } = string.Empty;
        public string UrlRequest { get; set; } = string.Empty;

        public string CodigoErrorCedula { get; set; } = string.Empty;
        public string EstadoCedula { get; set; } = string.Empty;
        public string DepartamentoExpedicionDocumento { get; set; } = string.Empty;
        public string FechaExpedicionDocumento { get; set; } = string.Empty;
        public string MunicipioExpedicionDocumento { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = string.Empty;
        public string SegundoNombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
    }
}
