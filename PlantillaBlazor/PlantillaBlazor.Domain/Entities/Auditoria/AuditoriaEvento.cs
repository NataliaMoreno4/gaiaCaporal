using PlantillaBlazor.Domain.Entities.Perfilamiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    /// <summary>
    /// Modelo que representa un registro de auditoría de eventos dentro del aplicativo
    /// </summary>
    [Table("AuditoriaEventos", Schema = "Aud")]
    public class AuditoriaEvento : BaseEntity
    {
        /// <summary>
        /// Acción realizada
        /// </summary>
        public required string Accion { get; set; } = string.Empty;
        /// <summary>
        /// Descripción de la acción realiada
        /// </summary>
        [MaxLength(200)]
        public required string Descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del usuario que generó la acción
        /// </summary>
        public required long IdUsuarioAccion { get; set; }
        /// <summary>
        /// Referencia al usuario que generó la acción
        /// </summary>
        public virtual Usuario UsuarioAccion { get; set; }
        /// <summary>
        /// Dirección IP desde la cual se generó el registro de auditoría
        /// </summary>
        [MaxLength(30)]
        public required string IpAddress { get; set; } = string.Empty;
        /// <summary>
        /// Concepto para el cual se generó el registro de auditoría (Rol, Usuario, Modulo, etc)
        /// </summary>
        [MaxLength(200)]
        public required string Concepto { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del proceso para el cual se generó el registro de auditoría
        /// </summary>
        [MaxLength(200)]
        public required string IdentificadorProceso { get; set; } = string.Empty;
    }
}
