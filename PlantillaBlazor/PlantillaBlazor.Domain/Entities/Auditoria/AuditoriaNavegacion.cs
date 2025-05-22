using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Entities.Auditoria
{
    /// <summary>
    /// Modelo que representa un registro de auditoría de navegacion dentro del aplicativo
    /// </summary>
    [Table("AuditoriaNavegacion", Schema = "Aud")]
    public class AuditoriaNavegacion : BaseEntity
    {
        public string UserAgent { get; set; } = string.Empty;
        public string Navegador { get; set; } = string.Empty;
        public string VersionNavegador { get; set; } = string.Empty;
        public string PlataformaNavegador { get; set; } = string.Empty;
        public string UrlActual { get; set; } = string.Empty;
        public string Idioma { get; set; } = string.Empty;
        public string CookiesHabilitadas { get; set; } = string.Empty;
        public string AnchoPantalla { get; set; } = string.Empty;
        public string AltoPantalla { get; set; } = string.Empty;
        public string ProfundidadColor { get; set; } = string.Empty;
        public string NombreSO { get; set; } = string.Empty;
        public string VersionSO { get; set; } = string.Empty;
        public string Latitud { get; set; } = string.Empty;
        public string Longitud { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string IdUsuarioAccion { get; set; } = string.Empty;
        public string IsLocationPermitted { get; set; } = string.Empty;
    }
}
