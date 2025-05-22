using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.Entities.GaiaCaporal;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Repositories.Implementations.GaiaCaporal;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Perfilamiento;
using PlantillaBlazor.Services.Interfaces.GaiaCaporal;
using System.Linq.Expressions;

namespace PlantillaBlazor.Services.Implementations.GaiaCaporal
{
    public class GaiaCaporalService : IGaiaCaporalService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public GaiaCaporalService
        (
            IProductoRepository productoRepository,
            IUsuarioRepository usuarioRepository
        )
        {
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<IEnumerable<Producto>> GetProductos()
        {
            var includes = new List<Expression<Func<Producto, object>>>();

            includes.Add(u => u.Categoria);
            includes.Add(u => u.Mercado);

            return await _productoRepository.Get(includes: includes);
        }
        public async Task<Result<long>> InsertarProducto(Producto producto)
        {

            Usuario usuarioMercado = await _usuarioRepository.GetById(producto.IdMercado);

            if (usuarioMercado is null)
            {
                return Result<long>.Failure("No es posible guardar la información. El mercado asociado no se encuentra");
            }

            if (!await _productoRepository.InsertarProducto(producto))
            {
                return Result<long>.Failure("No es posible guardar la información del producto");
            }

            return Result<long>.Success(producto.Id);
        }

    }
}
