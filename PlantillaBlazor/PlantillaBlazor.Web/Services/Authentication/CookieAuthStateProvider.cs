using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Security.Claims;

namespace PlantillaBlazor.Web.Services.Authentication
{
    public class CookieAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<CookieAuthStateProvider> _logger;

        public CookieAuthStateProvider(ILoggerFactory loggerFactory, ISessionService sessionService, ILogger<CookieAuthStateProvider> logger) : base(loggerFactory)
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var user = authenticationState?.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var sessionclaim = user.Claims.Where(c => c.Type == "IdSession").FirstOrDefault();
                var sessionId = sessionclaim?.Value;

                if (!string.IsNullOrEmpty(sessionId))
                {
                    var session = await _sessionService.GetSessionById(long.Parse(sessionId));

                    if (session is not null)
                    {
                        //_logger.LogInformation($"Session {session.Id}: Active {session.IsActive}");
                        return session.IsActive;
                    }
                }
            }

            return false;
        }
    }
}
