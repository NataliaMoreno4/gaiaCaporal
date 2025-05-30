using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlantillaBlazor.Domain.Common.Options.Email;
using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Email;

namespace PlantillaBlazor.Services.Implementations.Email
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IEmailService"/>
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpOptions _smtpOptions;
        private readonly IAuditoriaService _auditoriaService;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger,
            IOptions<SmtpOptions> smtpOptionsService,
            IAuditoriaService auditoriaService
        )
        {
            _configuration = configuration;
            _logger = logger;
            _smtpOptions = smtpOptionsService.Value;
            _auditoriaService = auditoriaService;
        }

    }
}
