namespace PlantillaBlazor.Services.Interfaces.Encrypt
{
    /// <summary>
    /// Representa la interfaz mediante la cual se expondrán diferentes operaciones de encriptación
    /// </summary>
    public interface IEncryptService
    {
        public bool EncriptarPDF(string rutaArchivoOriginal, string rutaArchivoFinal, string contraseña);
        /// <summary>
        /// Convierte un objeto diccionario de tipo <see cref="Dictionary{string, string}"/> en un string base64
        /// </summary>
        /// <param name="parametros">Diccionario el cual contiene los distintos parámetros que serán encryptados</param>
        /// <returns>Un string base64</returns>
        public string EncriptarParametros(Dictionary<string, string> parametros);
        /// <summary>
        /// Convierte un string base64 previamente encriptado en un objeto diccionario de tipo <see cref="Dictionary{string, string}"/>
        /// </summary>
        /// <param name="parametrosEncriptadosBase64">String base64</param>
        /// <returns>Objeto <see cref="Dictionary{string, string}"/></returns>
        public Dictionary<string, string> DesencriptarParametros(string parametrosEncriptadosBase64);
    }
}
