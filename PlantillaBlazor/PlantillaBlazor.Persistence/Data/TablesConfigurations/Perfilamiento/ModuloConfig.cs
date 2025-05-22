using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantillaBlazor.Domain.Entities.Perfilamiento;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento
{
    public class ModuloConfig : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            //Data inicial
            builder.HasData(
                Build()
            );
        }

        private List<Modulo> Build()
        {
            return new List<Modulo>()
            {
                new Modulo()
                {
                    Id = 1,
                    NombreModulo = "Configuración",
                    TipoModulo = "Módulo",
                    Nivel = "1",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo()
                {
                    Id = 2,
                    NombreModulo = "Perfilamiento",
                    TipoModulo = "Submódulo",
                    Nivel = "1.1",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo(){
                    Id = 3,
                    NombreModulo = "Usuarios",
                    TipoModulo = "Submódulo",
                    Nivel = "1.1.1",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo()
                {
                    Id = 4,
                    NombreModulo = "Roles",
                    TipoModulo = "Submódulo",
                    Nivel = "1.1.2",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo()
                {
                    Id = 5,
                    NombreModulo = "Módulos",
                    TipoModulo = "Submódulo",
                    Nivel = "1.1.3",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo()
                {
                    Id = 6,
                    NombreModulo = "Parametrización",
                    TipoModulo = "Submódulo",
                    Nivel = "1.2",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
                new Modulo()
                {
                    Id = 7,
                    NombreModulo = "Productos",
                    TipoModulo = "Módulo",
                    Nivel = "2",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1)
                },
            };
        }
    }
}
