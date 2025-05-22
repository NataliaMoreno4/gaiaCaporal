using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities
{
    /// <summary>
    /// Entidad base
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador autoincrementable
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Fecha en la que se creó la entidad
        /// </summary>
        public DateTime FechaAdicion { get; set; }
        /// <summary>
        /// Fecha en la que la entidad fue actualizada por última vez
        /// </summary>
        public DateTime? FechaUltimaActualizacion { get; set; }
        /// <summary>
        /// Id del usuario que creó la entidad
        /// </summary>
        public long? IdUsuarioAdiciono { get; set; }
        /// <summary>
        /// Id del usuario que modificó la entidad por última vez
        /// </summary>
        public long? IdUsuarioUltimaActualizacion { get; set; }
    }
}
