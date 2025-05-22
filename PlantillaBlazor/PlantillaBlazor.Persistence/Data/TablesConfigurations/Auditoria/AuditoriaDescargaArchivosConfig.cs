using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Auditoria
{
    public class AuditoriaDescargaArchivosConfig : IEntityTypeConfiguration<AuditoriaDescargaArchivo>
    {
        public void Configure(EntityTypeBuilder<AuditoriaDescargaArchivo> builder)
        {

        }
    }
}
