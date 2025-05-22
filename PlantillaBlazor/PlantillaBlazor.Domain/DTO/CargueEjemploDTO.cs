using PlantillaBlazor.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.DTO
{
    public class CargueEjemploDTO
    {
        public long IdUsuario { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public Archivo ArchivoCargue { get; set; }
    }
}
