using PlantillaBlazor.Domain.Common.Options.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Options.AppConfig
{
    public class AppConfigOptionsSetup : IConfigureOptions<AppConfigOptions>
    {
        private readonly IConfiguration _configuration;
        private const string ConfigurationSectionName = "AppConfigOptions";

        public AppConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(AppConfigOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
