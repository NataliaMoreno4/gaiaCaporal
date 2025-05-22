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
    public class RolModuloConfig : IEntityTypeConfiguration<RolModulo>
    {
        public void Configure(EntityTypeBuilder<RolModulo> builder)
        {
            //FK rol
            builder.HasOne(rm => rm.Rol)
                .WithMany(r => r.ListaRolModulo)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(rm => rm.IdRol);

            //FK modulo
            builder.HasOne(rm => rm.Modulo)
                .WithMany(m => m.ListaRolModulo)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(rm => rm.IdModulo);

            //Data inicial
            builder.HasData(
                Build()
            );
        }

        private List<RolModulo> Build()
        {
            return new List<RolModulo>()
            {
                new RolModulo
                {
                    Id = 1,
                    IdRol = 1,
                    IdModulo = 1,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new RolModulo
                {
                    Id = 2,
                    IdRol = 1,
                    IdModulo = 2,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new RolModulo
                {
                    Id = 3,
                    IdRol = 1,
                    IdModulo = 3,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new RolModulo
                {
                    Id = 4,
                    IdRol = 1,
                    IdModulo = 4,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new RolModulo
                {
                    Id = 5,
                    IdRol = 1,
                    IdModulo = 5,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new RolModulo
                {
                    Id = 6,
                    IdRol = 1,
                    IdModulo = 6,
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                }
            };
        }
    }
}
