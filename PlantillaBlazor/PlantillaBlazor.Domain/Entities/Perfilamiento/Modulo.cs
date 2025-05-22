using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Perfilamiento
{
    /// <summary>
    /// Modelo que representa la información de un módulo
    /// </summary>
    [Table("Modulo", Schema = "Seg")]
    public class Modulo : BaseEntity
    {
        /// <summary>
        /// Nombre del módulo
        /// </summary>
        [MaxLength(50)]
        public string NombreModulo { get; set; } = string.Empty;
        /// <summary>
        /// Tipo de módulo (Módulo o submódulo)
        /// </summary>
        [MaxLength(30)]
        public string TipoModulo { get; set; } = string.Empty;
        /// <summary>
        /// Nivel de jerarquía del módulo
        /// </summary>
        [MaxLength(20)]
        public string Nivel { get; set; } = string.Empty;
        /// <summary>
        /// Lista de asignaciones rol-módulo
        /// </summary>
        public virtual List<RolModulo> ListaRolModulo { get; set; } = new List<RolModulo>();
    }
}
