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
    /// Modelo que representa la información de la una asignación Rol-Modulo
    /// </summary>
    [Table("RolModulo", Schema = "Seg")]
    public class RolModulo : BaseEntity
    {
        /// <summary>
        /// Identificador del rol asociado
        /// </summary>
        public long IdRol { get; set; }
        /// <summary>
        /// Referencia al rol asociado
        /// </summary>
        public virtual Rol Rol { get; set; }
        /// <summary>
        /// Identificador del módulo asociado
        /// </summary>
        public long IdModulo { get; set; }
        /// <summary>
        /// Referencia al módulo asociado
        /// </summary>
        public virtual Modulo Modulo { get; set; }
    }
}
