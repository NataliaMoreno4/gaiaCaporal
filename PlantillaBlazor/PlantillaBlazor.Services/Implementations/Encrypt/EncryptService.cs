
using Encriptacion;
using iText.Kernel.Pdf;
using PlantillaBlazor.Services.Interfaces.Encrypt;

namespace PlantillaBlazor.Services.Implementations.Encrypt
{
    public class EncryptService : IEncryptService
    {
        private readonly EncriptadorUtility _encriptionUtility;

        public EncryptService()
        {
            _encriptionUtility = new EncriptadorUtility();
        }

        public string EncriptarParametros(Dictionary<string, string> parametros)
        {
            return _encriptionUtility.encriptarParametros(parametros);
        }

        public Dictionary<string, string> DesencriptarParametros(string parametrosEncriptadosBase64)
        {
            return _encriptionUtility.desencriptarParametros(parametrosEncriptadosBase64);
        }

        public bool EncriptarPDF(string rutaArchivoOriginal, string rutaArchivoFinal, string contraseña)
        {
            try
            {
                // Cargar el PDF
                PdfReader pdfReader = new PdfReader(rutaArchivoOriginal);

                // Crear el archivo PDF de salida
                PdfWriter pdfWriter = new PdfWriter(rutaArchivoFinal, new WriterProperties()
                    .SetStandardEncryption(
                        System.Text.Encoding.UTF8.GetBytes(contraseña),
                        System.Text.Encoding.UTF8.GetBytes(contraseña),
                        EncryptionConstants.ALLOW_PRINTING,
                        EncryptionConstants.ENCRYPTION_AES_256
                    )
                );

                // Crear el documento PDF
                PdfDocument pdfDocument = new PdfDocument(pdfReader, pdfWriter);
                pdfDocument.Close();

                pdfReader.Close();
                pdfWriter.Close();

                return File.Exists(rutaArchivoFinal);
            }
            catch (Exception exe)
            {
                Serilog.Log.Error(exe, $"Error al encriptar PDF");
            }

            return false;
        }
    }
}
