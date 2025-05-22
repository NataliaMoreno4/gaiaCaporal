using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Domain.Common.Options.Email
{
    public class SmtpOptionsSetup : IConfigureOptions<SmtpOptions>
    {
        private readonly IConfiguration _configuration;
        private const string ConfigurationSectionName = "SmtpOptions";

        public SmtpOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(SmtpOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
