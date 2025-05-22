using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFUtilities
{
    /// <summary>
    /// Realiza distintas operaciones de estampado sobre PDFs
    /// </summary>
    public class CustomPDFStamper
    {
        /// <summary>
        /// Dado una lista de elementos de firma <see cref="SignElement"/>, este método estampa dichos elementos en un documento
        /// PDF mediante <see langword="iText7"/>
        /// </summary>
        /// <param name="elementosFirma">Lista de elementos de firma que serán estampados</param>
        /// <param name="documentoOriginal">Ruta absoluta del documento sobre el cual serán estampados los elementos de firma</param>
        /// <param name="documentoFinal">Ruta absoluta del documento resultante</param>
        /// <returns></returns>
        public bool EstamparElementosFirmaPDF(IEnumerable<SignElement> elementosFirma,string documentoOriginal, string documentoFinal)
        {
            string rutaCross = Path.Combine(Directory.GetCurrentDirectory(), "assetsFirmaDigital", "cross-mark.png");
            string rutaCheck = Path.Combine(Directory.GetCurrentDirectory(), "assetsFirmaDigital", "check.png");
            string rutaRadio = Path.Combine(Directory.GetCurrentDirectory(), "assetsFirmaDigital", "radio-btn.png");
            string rutaSello = Path.Combine(Directory.GetCurrentDirectory(), "assetsFirmaDigital", "sello.png");

            string rutaDocumentosTemporales = Path.Combine(Directory.GetCurrentDirectory(), "archivosTemporales");
            Directory.CreateDirectory(rutaDocumentosTemporales);

            PdfDocument pdfDocument = new PdfDocument(new PdfReader(documentoOriginal), new PdfWriter(documentoFinal));

            Document document = new Document(pdfDocument);

            int x = 0;
            int y = 0;
            int pagina = 0;
            int height = 0;
            int width = 0;
            int fontSize = 0;
            int lineHeight = 0;

            foreach (var elemento in elementosFirma.Where(f => !f.Tipo.Equals("Espacio de firma")))
            {
                pagina = elemento.NumeroPagina;

                iText.Kernel.Geom.Rectangle rec = pdfDocument.GetPage(pagina).GetPageSize();

                width = (int)Math.Round(double.Parse(elemento.Width, CultureInfo.InvariantCulture), 0);
                height = (int)Math.Round(double.Parse(elemento.Height, CultureInfo.InvariantCulture), 0);
                x = (int)Math.Round(double.Parse(elemento.CoordenadaX, CultureInfo.InvariantCulture), 0) + 1;
                y = (int)Math.Round(double.Parse(elemento.CoordenadaY, CultureInfo.InvariantCulture), 0) + 1;

                y = (int)rec.GetHeight() - y - height;

                string contenido = elemento.Contenido;

                if (elemento.Tipo.Equals("texto"))
                {
                    fontSize = (int)Math.Round(double.Parse(elemento.FontSize, CultureInfo.InvariantCulture), 0);
                    lineHeight = (int)Math.Round(double.Parse(elemento.LineHeight, CultureInfo.InvariantCulture), 0);

                    var p = new Paragraph(contenido)
                        .SetFontSize(fontSize)
                        .SetFixedPosition(x, y, width + 100)
                        .SetPaddings(0, 0, 0, 0)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.BOTTOM)
                        .SetPageNumber(pagina)
                        .SetFixedLeading(lineHeight);

                    document.Add(p);

                    continue;
                }

                string ruta_imagen = "";

                switch (elemento.Tipo)
                {
                    case "firma":
                        string base_64 = contenido.Replace("data:image/png;base64,", "");
                        byte[] bytes = Convert.FromBase64String(base_64);

                        string ruta_final_firma = Path.Combine(rutaDocumentosTemporales, $"Firma_{Guid.NewGuid().ToString("N")}.png");

                        File.WriteAllBytes(ruta_final_firma, bytes);

                        ruta_imagen = ruta_final_firma;

                        break;
                    case "check":
                        ruta_imagen = rutaCheck;
                        break;
                    case "cross":
                        ruta_imagen = rutaCross;
                        break;
                    case "radio":
                        ruta_imagen = rutaRadio;
                        break;
                }


                ImageData imageData = ImageDataFactory.Create(ruta_imagen);

                Image image = new Image(imageData)
                    .ScaleAbsolute(width, height)
                    .SetFixedPosition(pagina, x, y);
                // This adds the image to the page
                document.Add(image);
            }
            document.Close();

            return File.Exists(documentoFinal);
        }

        public static bool PlaceImageOnPdf(string rutaOrignal, string rutaFinal, string rutaImagen, int x, int y, int pagina, int height, int width)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(rutaOrignal), new PdfWriter(rutaFinal));

            Document document = new Document(pdfDocument);

            ImageData imageData = ImageDataFactory.Create(rutaImagen);

            Image image = new Image(imageData)
                .ScaleAbsolute(width, height)
                .SetFixedPosition(pagina, x, y);
            // This adds the image to the page
            document.Add(image);

            document.Close();

            return File.Exists(rutaFinal);
        }
    }
}
