using Microsoft.EntityFrameworkCore;
using PlantillaBlazor.Domain.Entities.GaiaCaporal;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Persistence.Repositories.Common;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.GaiaCaporal
{
    public class ProductoRepository : EFCoreRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(IDbContextFactory<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<bool> InsertarProducto(Producto producto)
        {
            using var context = _dbContextFactory.CreateDbContext();

            Producto temp = context.Productos
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == producto.Id);

            if (temp is null)
            {
                producto.FechaAdicion = DateTime.Now;
                context.Productos.Add(producto);
            }
            else
            {
                producto.FechaUltimaActualizacion = DateTime.Now;
                context.Productos.Update(producto);
            }

            int entities = await context.SaveChangesAsync();

            return entities > 0;
        }
    }
}
