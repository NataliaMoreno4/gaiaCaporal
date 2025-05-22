using System.ComponentModel.DataAnnotations;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    public abstract class AuditoriaConsumoAPI : BaseEntity
    {
        public DateTime? FechaInicioConsulta { get; set; }
        public DateTime? FechaFinConsulta { get; set; }
        [MaxLength(100)]
        public string StatusCodeRespuesta { get; set; } = string.Empty;
        public string BodyRequest { get; set; } = string.Empty;
        public string JsonResponse { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        [MaxLength(200)]
        public string IpConsulta { get; set; } = string.Empty;
        [MaxLength(200)]
        public string UsuarioConsulta { get; set; } = string.Empty;
        public string UrlRequest { get; set; } = string.Empty;
        public string IdentificadorProceso { get; set; } = string.Empty;
        public string TipoProceso { get; set; } = string.Empty;
    }
}
