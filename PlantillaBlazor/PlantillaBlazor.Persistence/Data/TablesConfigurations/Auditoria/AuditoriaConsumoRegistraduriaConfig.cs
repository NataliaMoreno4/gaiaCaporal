using PlantillaBlazor.Domain.Entities.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Auditoria
{
    public class AuditoriaConsumoRegistraduriaConfig : IEntityTypeConfiguration<AuditoriaConsumoRegistraduria>
    {
        public void Configure(EntityTypeBuilder<AuditoriaConsumoRegistraduria> builder)
        {
            builder.HasIndex(a => a.CedulaConsultada);
            builder.HasIndex(a => a.UsuarioConsulta);
            builder.HasIndex(a => a.StatusCodeRespuesta);
            builder.HasIndex(a => a.CodigoErrorCedula);
            builder.HasIndex(a => a.EstadoCedula);
        }
    }
}
