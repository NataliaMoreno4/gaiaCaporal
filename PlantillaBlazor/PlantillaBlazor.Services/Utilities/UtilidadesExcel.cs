using ExcelDataReader;
using NPOI.SS.UserModel;

namespace PlantillaBlazor.Services.Utilities
{
    public class UtilidadesExcel
    {
        /// <summary>
        /// Obtiene la cantidad de registros que contiene un hoja de un archivo excel
        /// </summary>
        /// <param name="rutaArchivo">Ruta absoluta del archivo excel</param>
        /// <param name="hoja">Hoja del archivo excel de la que se quieren contar los registros</param>
        /// <returns>Cantidad de registros</returns>
        public static long CalcularTotalRegistrosHojaExcel(string rutaArchivo, int hoja)
        {
            long totalRegistros = 0;

            using (var stream = File.Open(rutaArchivo, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    totalRegistros = result.Tables[hoja].Rows.Count - 1;
                }
            }

            return totalRegistros;
        }

        /// <summary>
        /// Crea un estilo para una celda de un archivo Excel creado o editado con NPOI
        /// </summary>
        /// <param name="workbook">Libro de excel sobre el que se va aplicar el estilo</param>
        /// <param name="colorFondo">Color de la celda en hexadecimal</param>
        /// <param name="colorLetra">Color de la letra de la celda</param>
        /// <returns>Estilo de celda</returns>
        public static NPOI.XSSF.UserModel.XSSFCellStyle CreateCellStyle(IWorkbook workbook, string colorFondo, string colorLetra = "#fff")
        {
            var colorRGB = System.Drawing.ColorTranslator.FromHtml(colorFondo);

            //Estilo para las celdas de encabezado
            byte[] rgb = new byte[3] { colorRGB.R, colorRGB.G, colorRGB.B };
            NPOI.XSSF.UserModel.XSSFColor color = new NPOI.XSSF.UserModel.XSSFColor(rgb);

            NPOI.XSSF.UserModel.XSSFCellStyle boldStyle = (NPOI.XSSF.UserModel.XSSFCellStyle)workbook.CreateCellStyle();
            boldStyle.SetFillForegroundColor(color);
            boldStyle.FillPattern = FillPattern.SolidForeground;
            boldStyle.Alignment = HorizontalAlignment.Center;
            boldStyle.VerticalAlignment = VerticalAlignment.Center;

            IFont font = workbook.CreateFont();
            font.Color = NPOI.SS.UserModel.IndexedColors.White.Index;
            boldStyle.SetFont(font);

            return boldStyle;
        }
        /// <summary>
        /// Obtiene el valor en cadena de texto de una celda específica
        /// </summary>
        /// <param name="columna">Columna de la celda</param>
        /// <param name="fila">Fila de la celda</param>
        /// <returns></returns>
        public static string GetStringCellValue(IExcelDataReader reader, int columna)
        {
            string value = "";

            try
            {
                value = reader.GetValue(columna).ToString();
            }
            catch (Exception exe)
            {
            }

            return value;
        }

        /// <summary>
        /// Obtiene el valor de tipo fecha de una celda específica de un archivo Excel usando NPOI
        /// </summary>
        /// <param name="fila">Número de la fila</param>
        /// <param name="columna">Número de la columna</param>
        /// <returns>Valor en fecha de la celda</returns>
        public static DateTime? GetDateCellValue(IExcelDataReader reader, int columna)
        {
            DateTime? fecha = null;

            try
            {
                fecha = reader.GetDateTime(columna);
            }
            catch (Exception ex)
            {

            }

            return fecha;
        }

    }
}
