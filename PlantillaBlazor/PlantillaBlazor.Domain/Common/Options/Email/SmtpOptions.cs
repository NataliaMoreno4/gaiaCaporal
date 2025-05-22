using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Options.Email
{
    /// <summary>
    /// Opciones de configuración para una bandeja SMTP
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// Bandeja de correo que realizará los envíos
        /// </summary>
        public string EmailFrom { get; set; } = string.Empty;
        /// <summary>
        /// Host o dirección IP del sservidor SMTP
        /// </summary>
        public string EmailHost { get; set; } = string.Empty;
        /// <summary>
        /// Dirección de correo electrónico a la que se copiará en oculto en todos los correos que sean enviados
        /// </summary>
        public string EmailBCC { get; set; } = string.Empty;
        /// <summary>
        /// Puerto para realizar la conexión SMTP
        /// </summary>
        public int EmailPort { get; set; }
        /// <summary>
        /// Contraseña de la bandeja SMTP
        /// </summary>
        public string EmailPass { get; set; } = string.Empty;
        /// <summary>
        /// Indica si los envíos de correo se realizarán mediante SSL
        /// </summary>
        public bool EmailSSLEnabled { get; set; }
    }
}
