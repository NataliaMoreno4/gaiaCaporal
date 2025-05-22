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
    /// Modelo que representa la información de un rol
    /// </summary>
    [Table("Rol", Schema = "Seg")]
    public class Rol : BaseEntity
    {
        /// <summary>
        /// Nombre del rol
        /// </summary>
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        /// <summary>
        /// Identifica si el rol está activo o no
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Lista de asignaciones rol-modulo
        /// </summary>
        public virtual List<RolModulo> ListaRolModulo { get; set; } = new List<RolModulo>();
        /// <summary>
        /// Lista de usuarios que están ligados al rol
        /// </summary>
        public virtual List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
