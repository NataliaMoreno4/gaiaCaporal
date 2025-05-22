using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Forms.Form.Element;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;
using static iText.Layout.Properties.BackgroundRepeat;
using iText.Layout;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Pdfa;
using Serilog;
using iText.Forms.Fields.Properties;
using iText.Kernel.Geom;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;

namespace PDFUtilities
{
    public class FirmaDigitalUtility
    {
        /// <summary>
        /// Campo de firma utilizado en las distintas operaciones de firmas digitales
        /// </summary>
        private string _fieldName = "FirmaDigital";

        /// <summary>
        /// Determina si un documento PDF está en formato PDF/A
        /// </summary>
        /// <param name="pdfFilePath">Ruta absoluta del documento</param>
        /// <returns><see langword="true"/> en caso de que el PDF esté en formato PDF/A, <see langword="false"/> en caso de que no</returns>
        private bool IsPdfA(string pdfFilePath)
        {
            try
            {
                PdfReader reader = new PdfReader(pdfFilePath);
                PdfDocument pdfDoc = new PdfDocument(reader);

                PdfAConformanceLevel conformanceLevel = pdfDoc.GetReader().GetPdfAConformanceLevel();
                pdfDoc.Close();

                return conformanceLevel != null; // Si conformanceLevel es null, no es PDF/A; de lo contrario, es PDF/A.
            }
            catch (Exception ex)
            {
                return false; // En caso de error, asumimos que no es PDF/A.
            }
        }

        /// <summary>
        /// Realiza la firma digital de un documento mediate la librería <c>iText7</c>, con la posibilidad de imprimir una imagen junto a la firma digital
        /// </summary>
        /// <param name="properties">Objeto <see cref="FirmaProperties"/> el cual describe las diferentes características que tendrá la firma dentro del documento, como detalles del certificado, posición, tamaño, entre otros</param>
        /// <param name="appendMode">Determina si el documento después de firmado estará abierto a modificaciones. Por defecto su valor es <see langword="false" /></param>
        /// <param name="tipoFirma">Tipo de firma</param>
        /// <returns><see langword="true" /> si la firma se realizó correctamente, <see langword="false" /> en caso contrario</returns>
        public bool FirmarDocumentoImagen(FirmaProperties properties, bool appendMode = false)
        {
            string rutaTemp = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "Documentos",
                "Temporales",
                $"Temp_{Guid.NewGuid().ToString("N")}.pdf");

            FileInfo fi = new FileInfo(rutaTemp);
            fi.Directory.Create();

            //Soporte para documentos en formato PDF/A
            if (IsPdfA(properties.RutaDocumentoOriginal))
            {
                string rutaTempPdfa = System.IO.Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Documentos",
                    "Temporales",
                    $"Temp_PDFA_{Guid.NewGuid().ToString("N")}.pdf");

                ConvertPdfAtoPdfNormal(properties.RutaDocumentoOriginal, rutaTempPdfa);

                properties.RutaDocumentoOriginal = rutaTempPdfa;
            }

            //Firma digital
            var pdfSigner = new PdfSigner(new PdfReader(properties.RutaDocumentoOriginal), new FileStream(properties.RutaDocumentoFinal, FileMode.Create), new StampingProperties());

            pdfSigner.SetCertificationLevel(PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED);

            pdfSigner.SetFieldName(_fieldName);

            SignatureFieldAppearance appearance = null;

            if (File.Exists(properties.RutaImagenFirma))
            {
                appearance = new SignatureFieldAppearance(_fieldName)
                .SetContent(new SignedAppearanceText()
                    .SetReasonLine(properties.Reason)
                    .SetLocationLine(properties.Location), ImageDataFactory.Create(properties.RutaImagenFirma));
            }
            else
            {
                appearance = new SignatureFieldAppearance(_fieldName)
                .SetContent(new SignedAppearanceText()
                .SetReasonLine(properties.Reason)
                .SetLocationLine(properties.Location));
            }

            pdfSigner.SetSignatureAppearance(appearance);
            pdfSigner.SetPageNumber(properties.Pagina)
                .SetPageRect(new Rectangle(properties.CoordenadaX, properties.CoordenadaY, properties.Width, properties.Height))
                .SetReason(properties.Reason)
                .SetLocation(properties.Location);

            var password = properties.Certificado.ContraseñaCertificado.ToCharArray();
            IExternalSignature pks = GetPrivateKeySignature(properties.Certificado.RutaCertificado, password);
            var chain = GetCertificateChain(properties.Certificado.RutaCertificado, password);
            var ocspVerifier = new OCSPVerifier(null, null);
            var ocspClient = new OcspClientBouncyCastle(ocspVerifier);
            var crlClients = new List<ICrlClient>(new[] { new CrlClientOnline() });

            // Sign the document using the detached mode, CMS or CAdES equivalent. 
            pdfSigner.SignDetached(pks, chain, crlClients, ocspClient, null, 0,
                PdfSigner.CryptoStandard.CMS);

            return File.Exists(properties.RutaDocumentoFinal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="appendMode"></param>
        /// <returns></returns>
        public bool FirmarDocumentoImagenFondo(FirmaProperties properties, bool appendMode = false)
        {
            var pdfSigner = new PdfSigner(new PdfReader(properties.RutaDocumentoOriginal), new FileStream(properties.RutaDocumentoFinal, FileMode.Create), new StampingProperties());
            pdfSigner.SetCertificationLevel(PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED);

            BackgroundImage.Builder builder = new BackgroundImage.Builder();

            builder.SetImage(new PdfImageXObject(ImageDataFactory.Create(properties.RutaImagenFirma)));
            builder.SetBackgroundRepeat(new BackgroundRepeat(BackgroundRepeatValue.ROUND));

            BackgroundImage image = builder.Build();

            // Create the custom appearance as div.
            var customAppearance = new Div()
                .Add(new Paragraph($"Digitally Signed by {properties.NombreFirmante}\n{properties.Reason}\n{DateTime.Now.ToString("yyyy.MM.dd HH:mm.ss zzz")}")
                .SetFontSize(3)
                .SetCharacterSpacing(1)
                .SetMarginLeft(50)
                .SetMarginTop(7))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBackgroundImage(image);

            // Create the appearance instance and set the signature content to be shown and different appearance properties.
            var appearance = new SignatureFieldAppearance(_fieldName)
                .SetContent(customAppearance);

            // Set created signature appearance and other signer properties.
            pdfSigner
                .SetPageNumber(properties.Pagina)
                .SetPageRect(new Rectangle(properties.CoordenadaX, properties.CoordenadaY, properties.Width, properties.Height))
                .SetReason(properties.Reason)
                .SetLocation(properties.Location);

            pdfSigner.SetSignatureAppearance(appearance);

            var password = properties.Certificado.ContraseñaCertificado.ToCharArray();
            IExternalSignature pks = GetPrivateKeySignature(properties.Certificado.RutaCertificado, password);
            var chain = GetCertificateChain(properties.Certificado.RutaCertificado, password);
            var ocspVerifier = new OCSPVerifier(null, null);
            var ocspClient = new OcspClientBouncyCastle(ocspVerifier);
            var crlClients = new List<ICrlClient>(new[] { new CrlClientOnline() });

            // Sign the document using the detached mode, CMS or CAdES equivalent. 
            pdfSigner.SignDetached(pks, chain, crlClients, ocspClient, null, 0,
                PdfSigner.CryptoStandard.CMS);

            return File.Exists(properties.RutaDocumentoFinal);
        }

        /// <summary>Obtiene la llave privada de un certificado .pfx</summary>
        /// <param name="certificatePath">Certificate path</param>
        /// <param name="password">password</param>
        /// <returns>IPrivateKey instance to be used for the main signing operation.</returns>
        PrivateKeySignature GetPrivateKeySignature(string certificatePath, char[] password)
        {
            string? alias = null;
            var pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(certificatePath, FileMode.Open, FileAccess.Read), password);

            foreach (var a in pk12.Aliases)
            {
                alias = a;
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }

            IPrivateKey pk = new PrivateKeyBC(pk12.GetKey(alias).Key);
            return new PrivateKeySignature(pk, DigestAlgorithms.SHA512);
        }
        /// <summary>O</summary>
        /// <param name="certificatePath">Certificate path</param>
        /// <param name="password">password</param>
        /// <returns>The chain of certificates to be used for the signing operation.</returns>
        IX509Certificate[] GetCertificateChain(string certificatePath, char[] password)
        {
            string? alias = null;
            var pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(certificatePath, FileMode.Open, FileAccess.Read), password);

            foreach (var a in pk12.Aliases)
            {
                alias = a;
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }

            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            var chain = new IX509Certificate[ce.Length];
            for (var k = 0; k < ce.Length; ++k)
            {
                chain[k] = new X509CertificateBC(ce[k].Certificate);
            }

            return chain;
        }
        /// <summary>
        /// Convierte un PDF en formato PDF/A a formato PDF normal
        /// </summary>
        /// <param name="pdfAFilePath">Ruta absoluta del archivo PDF en formato PDF/A</param>
        /// <param name="pdfNormalFilePath">Ruta absoluta del archivo PDF final</param>
        public void ConvertPdfAtoPdfNormal(string pdfAFilePath, string pdfNormalFilePath)
        {
            try
            {
                // Abrir el documento PDF/A
                PdfReader reader = new PdfReader(pdfAFilePath);
                PdfDocument pdfADoc = new PdfDocument(reader);

                // Crear un nuevo documento PDF normal
                PdfWriter writer = new PdfWriter(pdfNormalFilePath);
                PdfDocument pdfNormalDoc = new PdfDocument(writer);

                // Copiar cada página del PDF/A al nuevo PDF normal
                int numPages = pdfADoc.GetNumberOfPages();
                for (int pageNum = 1; pageNum <= numPages; pageNum++)
                {
                    pdfNormalDoc.AddPage(pdfADoc.GetPage(pageNum).CopyTo(pdfNormalDoc));
                }

                // Cerrar los documentos PDF/A y PDF normal
                pdfADoc.Close();
                pdfNormalDoc.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al convertir el PDF/A a PDF normal");
            }
        }

        public bool FirmarDocumentoFirmaNoVisible(FirmaProperties properties, bool appendMode = false)
        {
            string rutaTemp = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "Documentos",
                "Temporales",
                $"Temp_{Guid.NewGuid().ToString("N")}.pdf");

            FileInfo fi = new FileInfo(rutaTemp);
            fi.Directory.Create();

            //Soporte para documentos en formato PDF/A
            if (IsPdfA(properties.RutaDocumentoOriginal))
            {
                string rutaTempPdfa = System.IO.Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Documentos",
                    "Temporales",
                    $"Temp_PDFA_{Guid.NewGuid().ToString("N")}.pdf");

                ConvertPdfAtoPdfNormal(properties.RutaDocumentoOriginal, rutaTempPdfa);

                properties.RutaDocumentoOriginal = rutaTempPdfa;
            }

            //Firma digital
            var pdfSigner = new PdfSigner(new PdfReader(properties.RutaDocumentoOriginal), new FileStream(properties.RutaDocumentoFinal, FileMode.Create), new StampingProperties());

            pdfSigner.SetCertificationLevel(PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED);

            pdfSigner.SetFieldName(_fieldName);

            var password = properties.Certificado.ContraseñaCertificado.ToCharArray();
            IExternalSignature pks = GetPrivateKeySignature(properties.Certificado.RutaCertificado, password);
            var chain = GetCertificateChain(properties.Certificado.RutaCertificado, password);
            var ocspVerifier = new OCSPVerifier(null, null);
            var ocspClient = new OcspClientBouncyCastle(ocspVerifier);
            var crlClients = new List<ICrlClient>(new[] { new CrlClientOnline() });

            // Sign the document using the detached mode, CMS or CAdES equivalent. 
            pdfSigner.SignDetached(pks, chain, crlClients, ocspClient, null, 0,
                PdfSigner.CryptoStandard.CMS);

            return File.Exists(properties.RutaDocumentoFinal);
        }

        public bool FirmarDocumentoCustomContent(FirmaProperties properties, bool appendMode = false)
        {
            var pdfSigner = new PdfSigner(new PdfReader(properties.RutaDocumentoOriginal), new FileStream(properties.RutaDocumentoFinal, FileMode.Create), new StampingProperties());
            pdfSigner.SetCertificationLevel(PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED);

            // Create the custom appearance as div.
            var customAppearance = new Div()
                .Add(new Paragraph($"Digitally Signed by {properties.NombreFirmante}\n{properties.Reason}\n{DateTime.Now.ToString("yyyy.MM.dd HH:mm.ss zzz")}")
                .SetFontSize(3)
                .SetCharacterSpacing(1)
                )
                .SetTextAlignment(TextAlignment.CENTER);

            // Create the appearance instance and set the signature content to be shown and different appearance properties.
            var appearance = new SignatureFieldAppearance(_fieldName)
                .SetContent(customAppearance);

            // Set created signature appearance and other signer properties.
            pdfSigner
                .SetPageNumber(properties.Pagina)
                .SetPageRect(new Rectangle(properties.CoordenadaX, properties.CoordenadaY, properties.Width, properties.Height))
                .SetReason(properties.Reason)
                .SetLocation(properties.Location);

            pdfSigner.SetSignatureAppearance(appearance);

            var password = properties.Certificado.ContraseñaCertificado.ToCharArray();
            IExternalSignature pks = GetPrivateKeySignature(properties.Certificado.RutaCertificado, password);
            var chain = GetCertificateChain(properties.Certificado.RutaCertificado, password);
            var ocspVerifier = new OCSPVerifier(null, null);
            var ocspClient = new OcspClientBouncyCastle(ocspVerifier);
            var crlClients = new List<ICrlClient>(new[] { new CrlClientOnline() });

            // Sign the document using the detached mode, CMS or CAdES equivalent. 
            pdfSigner.SignDetached(pks, chain, crlClients, ocspClient, null, 0,
                PdfSigner.CryptoStandard.CMS);

            return File.Exists(properties.RutaDocumentoFinal);
        }

        public int GetNumberOfPages(string rutaPdf)
        {
            using (PdfReader pdfReader = new PdfReader(rutaPdf))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    // Obtener la cantidad de páginas
                    int numberOfPages = pdfDocument.GetNumberOfPages();
                    return numberOfPages;
                }
            }
        }

    }
}
