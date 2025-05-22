using PlantillaBlazor.Domain.Entities.GaiaCaporal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantillaBlazor.Domain.Entities.Parametrizacion
{
    /// <summary>
    /// Modelo que representa la información de un parámetro detallado
    /// </summary>
    [Table("ParametroDetallado", Schema = "Par")]
    public class ParametroDetallado : BaseEntity
    {
        /// <summary>
        /// Nombre del parámetro detallado
        /// </summary>
        [MaxLength(300)]
        public string Nombre { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del parámetro general al cual está asociado el parámetro detallado actual
        /// </summary>
        public long IdParametroGeneral { get; set; }
        /// <summary>
        /// Referencia al parámetro general al cual está asociado el parámetro detallado actual
        /// </summary>
        public virtual ParametroGeneral ParametroGeneral { get; set; }
        public virtual List<Producto> ListaProductos { get; set; } = new();
    }
}
