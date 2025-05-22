
using System.ComponentModel.DataAnnotations;

namespace PlantillaBlazor.Domain.DTO.Perfilamiento
{
    public class CambioContraseñaModel
    {
        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas deben coincidir")]
        [DataType(DataType.Password)]
        public string ConfirmacionContraseña { get; set; } = string.Empty;
    }
}
