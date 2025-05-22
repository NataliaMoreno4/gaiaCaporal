
using System.ComponentModel.DataAnnotations;

namespace PlantillaBlazor.Domain.DTO.Perfilamiento
{
    public class ReestablecerContraseñaModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
