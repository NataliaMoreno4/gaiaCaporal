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
    public class AuditoriaEnvioSMSConfig : IEntityTypeConfiguration<AuditoriaEnvioSMS>
    {
        public void Configure(EntityTypeBuilder<AuditoriaEnvioSMS> builder)
        {
            builder.HasIndex(a => a.CelularDestinatario);
            builder.HasIndex(a => a.FechaEnvio);
            builder.HasIndex(a => a.FueEnviado);
            builder.HasIndex(a => a.IdentificacionProceso);
            builder.HasIndex(a => a.Pantalla);
        }
    }
}
