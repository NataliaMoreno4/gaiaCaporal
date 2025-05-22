using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.DTO.Reportes
{
    /// <summary>
    /// DTO para representar un registro de un reporte de tipo concepto-cantidad
    /// </summary>
    public class DataItemDTO
    {
        /// <summary>
        /// Concepto
        /// </summary>
        public string Concepto { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad
        /// </summary>
        public double Cantidad { get; set; } = 0;
    }
}
