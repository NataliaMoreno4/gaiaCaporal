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
                    Clave = "11CFA7558C0EB35E4E3F56C622D6E615C5E832E2847F9C32B4B3CD93012F94C0-79A4B19D31A094EED2D0A1AD62F3EDCE", //123
                    Email = "natmorenos42@gmail.com",
                    Celular = "3174575592",
                    IdRol = 1,
                    IsActive = true,
                    MustChangePassword = false,
                    FechaAdicion = new DateTime(2024,5,1)
                }
            };
        }
    }
}
