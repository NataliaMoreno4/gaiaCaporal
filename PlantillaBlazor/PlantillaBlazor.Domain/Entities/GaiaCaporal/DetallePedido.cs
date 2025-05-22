using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.GaiaCaporal
{
    /// <summary>
    /// Modelo que representa la información de un módulo
    /// </summary>
    [Table("DetallePedido", Schema = "Ope")]
    public class DetallePedido : BaseEntity
    {
        public long IdProducto { get; set; }
        public int cantidad { get; set; }
    }
}
