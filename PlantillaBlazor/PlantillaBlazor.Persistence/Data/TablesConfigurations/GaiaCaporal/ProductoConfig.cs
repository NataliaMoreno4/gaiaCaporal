using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlantillaBlazor.Domain.Entities.GaiaCaporal;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.GaiaCaporal
{
    internal class ProductoConfig : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {

        }

    }
}
