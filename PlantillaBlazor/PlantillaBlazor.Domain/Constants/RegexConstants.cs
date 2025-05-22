using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Constants
{
    /// <summary>
    /// Clase que contiene patrones regex que son usados con frecuencias
    /// </summary>
    public static class RegexConstants
    {
        /// <summary>
        /// Regex para validar direcciones email
        /// </summary>
        public const string EMAIL_REGEX = @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$";
        /// <summary>
        /// Regex que valida si una cadena de texto contiene caracteres especiales
        /// </summary>
        public const string CARACTERES_ESPECIALES_REGEX = @"[][""!@$%^&*(){}#-@:;<>,.?/+_=|'~\\]";
        /// <summary>
        /// Regex que valida si una cadena de texto tiene solo números
        /// </summary>
        public const string SOLO_NUMEROS_REGEX = "^[0-9]*$";
        /// <summary>
        /// Regex que valida si una cadena de texto tiene solo números
        /// </summary>
        public const string SOLO_NUMERO_REGEX = "^[0-9]*$";
        /// <summary>
        /// Regex que valida si una cadena de texto tiene solo letras
        /// </summary>
        public const string SOLO_LETRAS_REGEX = "^[a-zA-ZÀ-ÿ \u00f1\u00d1]*$";
        /// <summary>
        /// Regex que valida si una cadena de texto tiene solo letras y números
        /// </summary>
        public const string LETRAS_NUMEROS_REGEX = "^[a-zA-ZÀ-ÿ0-9 \u00f1\u00d1]*$";
        /// <summary>
        /// Regex para contraseñas que deban tener al menos 8 caracteres, un minúscula, una mayúscula, un número y un caracter especial 
        /// </summary>
        public const string REGEX_CONTRASEÑAS = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$";
    }
}
