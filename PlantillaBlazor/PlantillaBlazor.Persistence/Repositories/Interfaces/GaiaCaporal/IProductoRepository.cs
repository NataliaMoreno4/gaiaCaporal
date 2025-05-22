using PlantillaBlazor.Domain.Entities.GaiaCaporal;
using PlantillaBlazor.Persistence.Repositories.Common;

namespace PlantillaBlazor.Persistence.Repositories.Implementations.GaiaCaporal
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        public Task<bool> InsertarProducto(Producto producto);
    }
}
