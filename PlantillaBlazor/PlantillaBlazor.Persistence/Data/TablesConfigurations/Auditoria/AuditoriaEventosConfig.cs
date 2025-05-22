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
    public class AuditoriaEventosConfig : IEntityTypeConfiguration<AuditoriaEvento>
    {
        public void Configure(EntityTypeBuilder<AuditoriaEvento> builder)
        {
            builder.HasIndex(a => a.Accion);
            builder.HasIndex(a => a.Concepto);
            builder.HasIndex(a => a.IdentificadorProceso);

            builder.HasOne(a => a.UsuarioAccion)
                .WithMany(u => u.ListaAuditoriasEventos)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(a => a.IdUsuarioAccion);
        }
    }
}
