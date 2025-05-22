using Microsoft.AspNetCore.Components.Forms;
using PlantillaBlazor.Domain.Common;
using PlantillaBlazor.Domain.Common.ResultModels;
using Serilog;

namespace PlantillaBlazor.Web.Helpers
{
    /// <summary>
    /// Realiza tareas de gestión de archivos del lado del cliente
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Transforma una ruta absoluta dentro del servidor en una ruta que pueda ser consultada por el cliente
        /// </summary>
        /// <param name="rutaAbsoluta">Ruta absoluta dentro del servidor</param>
        /// <returns>Ruta del lado del cliente</returns>
        public static Result<string> ConvertirRutaAbsolutaCliente(string rutaAbsoluta)
        {
            if (!File.Exists(rutaAbsoluta))
            {
                return Result<string>.Failure("El archivo no existe");
            }

            string directorio_temp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");
            Directory.CreateDirectory(directorio_temp);

            string nuevaRuta = Path.Combine(directorio_temp, $"File_{Guid.NewGuid().ToString("N")}{Path.GetExtension(rutaAbsoluta)}");

            File.Copy(rutaAbsoluta, nuevaRuta);

            string rutaFinal = nuevaRuta.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "");

            return Result<string>.Success(rutaFinal);

        }

        /// <summary>
        /// Dado una lista de tipo <see cref="IReadOnlyList{IBrowserFile}"/>, cual proviene de un componente <see cref="InputFile"/> en el que un usuario puede subir uno o varios archivos, devuelve una lista de <see cref="Archivo"/> que contiene la información de todos los archivos seleccionados por el usuario. Cada uno de los archivos es guardado en una ruta temporal ubicada en el <c>wwwroot</c>
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static async Task<Result<IEnumerable<Archivo>>> getTempFiles(IReadOnlyList<IBrowserFile> files, string[] allowedExtensions, long maxFileSize = 999999999)
        {
            if (files is null)
            {
                return Result<IEnumerable<Archivo>>.Failure("No se ha adjuntado ningún archivo");
            }

            if (files.Count <= 0)
            {
                return Result<IEnumerable<Archivo>>.Failure("No hay ningún archivo disponible");
            }

            List<Archivo> archivos = new List<Archivo>();

            string directorio_temp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");
            Directory.CreateDirectory(directorio_temp);

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file.Name);

                if (!allowedExtensions.Contains(ext))
                {
                    return Result<IEnumerable<Archivo>>.Failure("Tipo de archivo no permitido");
                }

                if (file.Size > maxFileSize)
                {
                    return Result<IEnumerable<Archivo>>.Failure("El tamaño máximo de subida ha sido excedido");
                }

                Archivo archivo = new Archivo();

                Stream stream = file.OpenReadStream(maxFileSize);

                string nombreArchivo = Path.GetFileNameWithoutExtension(file.Name);
                string nombre_final = nombreArchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + Guid.NewGuid().ToString("N") + ext;
                var path = Path.Combine(directorio_temp, nombre_final);
                FileStream fs = File.Create(path);
                try
                {
                    await stream.CopyToAsync(fs);

                    archivo.RutaAbsoluta = path;
                    archivo.RutaCliente = path.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "");
                    archivo.Extension = ext;
                    archivo.Nombre = nombreArchivo;

                    archivos.Add(archivo);
                }
                catch (Exception exe)
                {
                    Log.Error(exe, $"Error al guardar archivo");
                    return Result<IEnumerable<Archivo>>.Failure("Ha ocurrido en error al subir el archivo");
                }
                finally
                {
                    stream.Close();
                    fs.Close();
                }
            }

            return Result<IEnumerable<Archivo>>.Success(archivos);
        }
    }
}
