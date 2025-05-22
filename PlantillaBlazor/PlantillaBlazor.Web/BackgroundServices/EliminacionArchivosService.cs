
namespace PlantillaBlazor.Web.BackgroundServices
{
    public class EliminacionArchivosService : BackgroundService
    {
        private ILogger<EliminacionArchivosService> _logger;
        private IConfiguration _configuration;
        private static readonly TimeSpan Period = TimeSpan.FromHours(6);

        public EliminacionArchivosService(
            ILogger<EliminacionArchivosService> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _configuration = configuration;
        }


        private async Task EliminarArchivos()
        {
            _logger.LogInformation("Inicio de tarea de eliminación de archivos");

            await EliminarArchivosTemporales();

            _logger.LogInformation("Fin de tarea de eliminación de archivos");
        }

        /// <summary>
        /// Realiza la eliminación de archivos dentro de carpetas denominadas como temporales
        /// con duración de más de un 1 de haber sido creados dentro de la ubicación actual
        /// </summary>
        private async Task EliminarArchivosTemporales()
        {
            List<string> carpetas = new List<string>() { "wwwroot\\Temporales", "wwwroot\\archivosTemporales" };
            foreach (var carpeta in carpetas)
            {
                string ruta_final = Path.Combine(carpeta.Split("\\"));

                string path = Path.Combine(Directory.GetCurrentDirectory(), ruta_final);

                EliminarArchivoCarpeta(path);
            }

            await Task.Yield();
        }

        /// <summary>
        /// Elimmina todos los archivos de una carpeta con dias desde haber sido creados superior a un día
        /// </summary>
        /// <param name="path">Ruta absoluta de la carpeta en donde se eliminarán los archivos</param>
        private void EliminarArchivoCarpeta(string path)
        {
            if (!Directory.Exists(path)) return;

            List<string> files = Directory.GetFiles(path).ToList();

            foreach (string file in files)
            {
                try
                {
                    FileInfo fi = new FileInfo(file);

                    double dias = (DateTime.Now - fi.CreationTime).TotalDays;

                    if (dias >= 1)
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception exe)
                {

                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await EliminarArchivos();

                using var timer = new PeriodicTimer(Period);

                while (!stoppingToken.IsCancellationRequested &&
                       await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await EliminarArchivos();
                }
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (Exception exe)
            {
                _logger.LogError(exe, "Error al ejecutar tarea de eliminación de archivos");
            }

        }
    }
}
