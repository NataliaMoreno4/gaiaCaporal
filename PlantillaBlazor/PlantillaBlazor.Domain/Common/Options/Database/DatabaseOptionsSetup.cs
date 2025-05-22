using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PlantillaBlazor.Domain.Common.Options.Database
{
    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;
        private const string ConfigurationSectionName = "DatabaseOptions";

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("String de conexión no encontrado");

            options.ConnectionString = connectionString;

            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
