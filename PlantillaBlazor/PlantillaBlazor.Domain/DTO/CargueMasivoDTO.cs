using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.DTO
{
    public class CargueMasivoDTO
    {
        public long IdCargue { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public long TotalRegistrosArchivo { get; set; } = 0;
        public long TotalRegistrosLeidos { get; set; } = 0;
        public long TotalRegistrosProcesadosCorrecto { get; set; } = 0;
        public long TotalRegistrosProcesadosError { get; set; } = 0;
        public List<ErrorCargue> ListaErrores { get; set; } = new();
    }

    public class ErrorCargue
    {
        public long NumeroRegistro { get; set; }
        public string IdentificacionRegistro { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
