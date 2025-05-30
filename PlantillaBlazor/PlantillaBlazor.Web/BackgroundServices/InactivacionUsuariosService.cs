using PlantillaBlazor.Services.Interfaces.Perfilamiento;

namespace PlantillaBlazor.Web.BackgroundServices
{
    public class InactivacionUsuariosService : BackgroundService
    {
        private ILogger<InactivacionUsuariosService> _logger;
        private IConfiguration _configuration;
        private static readonly TimeSpan Period = TimeSpan.FromHours(24);
        private readonly IServiceProvider _serviceProvider;

        public InactivacionUsuariosService(
            ILogger<InactivacionUsuariosService> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider
        )
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }


        private async Task InactivarUsuarios()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();
                await userService.InactivarUsuariosNoActivos(90);
                await userService.InhabilitarSolicitudesCambioPassCaducadas(2);

                var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
                await sessionService.InhabilitarSesionesInactivas(2);
            }

            _logger.LogInformation($"Tarea de inactivación de usuarios realizada");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                using var timer = new PeriodicTimer(Period);

                while (!stoppingToken.IsCancellationRequested &&
                       await timer.WaitForNextTickAsync(stoppingToken))
                {
                }
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al ejecutar tarea de inactivación de usuarios");
            }

        }
    }
}
