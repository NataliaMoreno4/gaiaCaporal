using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.GaiaCaporal
{
    /// <summary>
    /// Modelo que representa la información de un módulo
    /// </summary>
    [Table("Producto", Schema = "Ope")]
    public class Producto : BaseEntity
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal? CostoUnitario { get; set; }
        public long IdCategoria { get; set; }
        public virtual ParametroDetallado? Categoria { get; set; }
        public long IdMercado { get; set; }
        public virtual Usuario? Mercado { get; set; }

    }
}
