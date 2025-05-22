using PlantillaBlazor.Domain.Entities.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Auditoria
{
    public class AuditoriaAdjuntoEmailConfig : IEntityTypeConfiguration<AuditoriaAdjuntoEmail>
    {
        public void Configure(EntityTypeBuilder<AuditoriaAdjuntoEmail> builder)
        {
            builder.HasIndex(a => a.RutaAbsolutaAdjunto);

            builder.HasOne(a => a.AuditoriaEnvioEmail)
                .WithMany(ae => ae.ListaAdjuntosEmail)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(a => a.IdAuditoriaEnvioEmail);
        }
    }
}
