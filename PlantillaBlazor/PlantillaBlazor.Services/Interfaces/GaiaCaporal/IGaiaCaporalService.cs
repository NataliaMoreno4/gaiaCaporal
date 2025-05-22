using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.GaiaCaporal;

namespace PlantillaBlazor.Services.Interfaces.GaiaCaporal
{
    public interface IGaiaCaporalService
    {
        public Task<Result<long>> InsertarProducto(Producto producto);
        public Task<IEnumerable<Producto>> GetProductos();
    }
}
