using PlantillaBlazor.Domain.Entities.Perfilamiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento
{
    public class AuditoriaLoginUsuarioConfig : IEntityTypeConfiguration<AuditoriaLoginUsuario>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLoginUsuario> builder)
        {
            builder.HasIndex(a => a.FechaAdicion);
            builder.HasIndex(a => a.Descripcion);

            builder.HasOne(a => a.Usuario)
                .WithMany(u => u.ListaAuditoriasLoginUsuario)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(a => a.IdUsuario);

            builder.Property(e => e.MotivoCierreSesion)
                .HasColumnType("nvarchar(max)");
        }
    }
}
