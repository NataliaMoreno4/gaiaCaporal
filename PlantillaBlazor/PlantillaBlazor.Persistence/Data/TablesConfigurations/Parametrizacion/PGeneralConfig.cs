using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantillaBlazor.Domain.Entities.Parametrizacion;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Parametrizacion
{
    public class PGeneralConfig : IEntityTypeConfiguration<ParametroGeneral>
    {
        public void Configure(EntityTypeBuilder<ParametroGeneral> builder)
        {
            builder.HasData(Build());
        }

        private List<ParametroGeneral> Build()
        {
            return new List<ParametroGeneral>()
            {
                new ParametroGeneral()
                {
                    Id = 1,
                    Nombre = "SiNo",
                    FechaAdicion = new DateTime(2024,5,1),
                    IdUsuarioAdiciono = 1
                },
                new ParametroGeneral()
                {
                    Id = 2,
                    Nombre = "CategoriaProducto",
                    FechaAdicion = new DateTime(2024,5,1),
                    IdUsuarioAdiciono = 1
                }
            };
        }
    }
}
