using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common
{
    /// <summary>
    /// Representa el modelo utilizado para describir un archivo
    /// </summary>
    public class Archivo
    {
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string Nombre { get; set; } = "";
        /// <summary>
        /// Extensión del archivo
        /// </summary>
        public string Extension { get; set; } = "";
        /// <summary>
        /// Ruta absoluta del archivo
        /// </summary>
        public string RutaAbsoluta { get; set; } = "";
        /// <summary>
        /// Ruta del cliente para el archivo
        /// </summary>
        public string RutaCliente { get; set; } = "";
    }
}
