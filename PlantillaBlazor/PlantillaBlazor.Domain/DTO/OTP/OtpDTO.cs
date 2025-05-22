using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.DTO.OTP
{
    /// <summary>
    /// Objeto DTO utilizado en el proceso de envío y verificación de códigos OTP
    /// </summary>
    public class OtpDto
    {
        /// <summary>
        /// Primer dígito del código OTP
        /// </summary>
        public string C1 { get; set; } = string.Empty;
        /// <summary>
        /// Segundo dígito del código OTP
        /// </summary>
        public string C2 { get; set; } = string.Empty;
        /// <summary>
        /// Tercer dígito del código OTP
        /// </summary>
        public string C3 { get; set; } = string.Empty;
        /// <summary>
        /// Cuarto dígito del código OTP
        /// </summary>
        public string C4 { get; set; } = string.Empty;
        /// <summary>
        /// Quinto dígito del código OTP
        /// </summary>
        public string C5 { get; set; } = string.Empty;
        /// <summary>
        /// Sexto dígito del código OTP
        /// </summary>
        public string C6 { get; set; } = string.Empty;
    }
}
