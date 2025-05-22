using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Parametrizacion
{
    /// <summary>
    /// Modelo que representa la información de un parámetro general
    /// </summary>
    [Table("ParametroGeneral", Schema = "Par")]
    public class ParametroGeneral : BaseEntity
    {
        /// <summary>
        /// Nombre del parámetro
        /// </summary>
        [MaxLength(300)]
        public string Nombre { get; set; } = string.Empty;
        /// <summary>
        /// Lista de parámetros detallados asociados
        /// </summary>
        public virtual List<ParametroDetallado> ListaParametrosDetallados { get; set; } = new List<ParametroDetallado>();
    }
}
