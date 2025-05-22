using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Options.AppConfig
{
    /// <summary>
    /// Representa la configuración del aplicativo en cuanto a cliente, logos, etc
    /// </summary>
    public class AppConfigOptions
    {
        /// <summary>
        /// Cliente para el cual está configurado el aplicativo
        /// </summary>
        public string NombreCliente { get; set; } = string.Empty;
        /// <summary>
        /// Nombre del aplicativo para el cual está configurado el aplicativo
        /// </summary>
        public string NombreAplicativo { get; set; } = string.Empty;
        /// <summary>
        /// Link del logo del cliente para el cual está configurado el aplicativo
        /// </summary>
        public string LinkLogoCorreos { get; set; } = string.Empty;
    }
}
