using System.ComponentModel.DataAnnotations;

namespace PlantillaBlazor.Domain.DTO.Perfilamiento
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
