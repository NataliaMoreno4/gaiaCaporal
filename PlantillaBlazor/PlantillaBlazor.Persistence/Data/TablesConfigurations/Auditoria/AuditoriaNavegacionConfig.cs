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
    public class AuditoriaNavegacionConfig : IEntityTypeConfiguration<AuditoriaNavegacion>
    {
        public void Configure(EntityTypeBuilder<AuditoriaNavegacion> builder)
        {
            builder.HasIndex(a => a.IdUsuarioAccion);
            builder.HasIndex(a => a.IpAddress);
            builder.HasIndex(a => a.UrlActual);
        }
    }
}
