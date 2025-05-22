
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento
{
    public class SessionsConfig : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            //Usuario
            builder.HasOne(s => s.Usuario)
                .WithMany(u => u.Sessiones)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(s => s.IdUsuario);

            builder.HasIndex(s => s.IsActive);
            builder.HasIndex(s => s.FechaUltimoIngreso);
            builder.HasIndex(s => s.IdAuditoriaLogin);
        }
    }
}
