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
    public class AuditoriaEnvioEmailConfig : IEntityTypeConfiguration<AuditoriaEnvioEmail>
    {
        public void Configure(EntityTypeBuilder<AuditoriaEnvioEmail> builder)
        {
            builder.HasIndex(e => e.EmailDestinatario);
            builder.HasIndex(e => e.FueEnviado);
            builder.HasIndex(e => e.NumeroIdentificacionProceso);
            builder.HasIndex(e => e.Concepto);
        }
    }
}
