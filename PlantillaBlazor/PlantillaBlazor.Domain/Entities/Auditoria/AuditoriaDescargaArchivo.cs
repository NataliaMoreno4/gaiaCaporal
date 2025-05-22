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
    /// Representa un registro de auditoría de descarga de archivo
    /// </summary>
    [Table("AuditoriaDescargaArchivos", Schema = "Aud")]
    public class AuditoriaDescargaArchivo : BaseEntity
    {
        /// <summary>
        /// Ruta absoluta dentro del servidor del archivo descargado
        /// </summary>
        public string RutaOriginal { get; set; } = string.Empty;
        /// <summary>
        /// Ruta cliente dentro del wwwroot la cual fue utilizada para descargar el archivo
        /// </summary>
        public string RutaDescargada { get; set; } = string.Empty;
        /// <summary>
        /// Nombre del archivo descargada
        /// </summary>
        public string NombreArchivo { get; set; } = string.Empty;
        /// <summary>
        /// Extensión del archivo descargado
        /// </summary>
        [MaxLength(20)]
        public string ExtensionArchivo { get; set; } = string.Empty;
        /// <summary>
        /// Peso del archivo descargado
        /// </summary>
        public long PesoArchivo { get; set; }
        /// <summary>
        /// Usuario que descarga el archivo
        /// </summary>
        public string Usuario { get; set; } = string.Empty;
        /// <summary>
        /// Dirección IP desde la cual se descargó el archivo
        /// </summary>
        [MaxLength(30)]
        public string IpAddress { get; set; } = string.Empty;
    }
}
