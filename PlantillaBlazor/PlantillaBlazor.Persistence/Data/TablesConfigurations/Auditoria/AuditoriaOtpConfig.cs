using PlantillaBlazor.Domain.Entities.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Auditoria
{
    public class AuditoriaOtpConfig : IEntityTypeConfiguration<AuditoriaOtp>
    {
        public void Configure(EntityTypeBuilder<AuditoriaOtp> builder)
        {
            builder.HasIndex(a => a.Codigo);
            builder.HasIndex(a => a.FechaAdicion);
            builder.HasIndex(a => a.FechaValidacion);
            builder.HasIndex(a => a.Estado);
            builder.HasIndex(a => a.IdentificacionProceso);
            builder.HasIndex(a => a.TipoProceso);
        }
    }
}
