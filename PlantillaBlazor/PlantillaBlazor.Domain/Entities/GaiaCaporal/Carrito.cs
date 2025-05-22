using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.GaiaCaporal
{
    /// <summary>
    /// Modelo que representa la información de un módulo
    /// </summary>
    [Table("Carrito", Schema = "Ope")]
    public class Carrito : BaseEntity
    {
        public decimal? CostoTotal { get; set; }
        public virtual List<Producto> ListaProductos { get; set; } = new List<Producto>();

    }
}
