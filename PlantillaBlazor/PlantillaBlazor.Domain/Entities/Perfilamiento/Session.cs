
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.Perfilamiento
{
    [Table("Session", Schema = "Seg")]
    public class Session : BaseEntity
    {
        public required long IdUsuario { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public required bool IsActive { get; set; } = true;
        public required bool RememberMe { get; set; } = true;
        public DateTime? FechaUltimoIngreso { get; set; }
        public DateTime? FechaInactivacion { get; set; }
        public string? Observaciones { get; set; }
        public required string? Host { get; set; }
        public required long? IdAuditoriaLogin { get; set; }
        public required string? IpAddress { get; set; }
        [MaxLength(50)]
        public string? TipoUsuario { get; set; } = "Normal";
    }
}
