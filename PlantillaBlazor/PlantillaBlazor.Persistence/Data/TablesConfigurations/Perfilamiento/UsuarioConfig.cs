using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantillaBlazor.Domain.Entities.Perfilamiento;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(u => u.IdRol);

            builder.HasIndex(u => u.IsActive);
            builder.HasIndex(u => u.NombreUsuario).IsUnique();
            builder.HasIndex(u => u.Email);

            //Data inicial
            builder.HasData(
                Build()
            );
        }

        private List<Usuario> Build()
        {
            return new List<Usuario>()
            {
                new Usuario()
                {
                    Id = 1,
                    Nombres = "Administrador",
                    Apellidos = "Administrador",
                    NombreUsuario = "Administrador",
                    Clave = "B9A465912169BEF97138C76EFDFD5BB34FDC5FA58855AC187817AE07E80ABE5E-5929B1B6239B2767DDEDDABC98823ADF", //123
                    Email = "leonardo.arias@excellentiam.co",
                    Celular = "3174575592",
                    IdRol = 1,
                    IsActive = true,
                    MustChangePassword = true,
                    FechaAdicion = new DateTime(2024,5,1)
                }
            };
        }
    }
}
