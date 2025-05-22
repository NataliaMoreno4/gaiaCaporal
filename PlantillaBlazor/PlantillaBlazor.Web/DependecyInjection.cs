using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PlantillaBlazor.Domain.Common;
using PlantillaBlazor.Domain.DTO.Perfilamiento;
using PlantillaBlazor.Services.Interfaces.Perfilamiento;
using PlantillaBlazor.Web.BackgroundServices;
using PlantillaBlazor.Web.Services;
using PlantillaBlazor.Web.Services.Authentication;
using PlantillaBlazor.Web.Services.Authorization;
using PlantillaBlazor.Web.Services.Circuits;
using PlantillaBlazor.Web.Utilidades;
using Radzen;
using System.Net;
using System.Security.Claims;

namespace PlantillaBlazor.Web
{
    public static class DependecyInjection
    {
        private static IServiceCollection ConfigAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                // Ignorar la validación antiforgery en el callback de autenticación
                options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
            });

            #region Autenticación y autorización

            bool isOpenIdConnectEnabled = configuration.GetSection("OpenIDConnectSettings:Enabled").Get<bool>();

            services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                if (isOpenIdConnectEnabled)
                    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                else
                    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "auth";
                o.Cookie.SameSite = SameSiteMode.Strict;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.MaxAge = TimeSpan.FromDays(14);
                o.Cookie.HttpOnly = true;
                o.Cookie.Path = "/";
                o.LoginPath = "/";
                o.AccessDeniedPath = "/access_denied";
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromDays(14);

                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = ctx =>
                    {
                        //if (ctx.Principal?.Identity?.IsAuthenticated ?? false)
                        //{
                        //    var claims = ctx.Principal?.Claims;

                        //    if (claims == null)
                        //    {
                        //        ctx.RejectPrincipal();
                        //        return ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        //    }
                        //    else
                        //    {
                        //        var sid = claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault();
                        //        var sidValue = sid?.Value ?? "";

                        //        if (sidValue != "555")
                        //        {
                        //            ctx.RejectPrincipal();
                        //            return ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        //        }
                        //    }
                        //}

                        return Task.CompletedTask;
                    }
                };
            });

            if (isOpenIdConnectEnabled)
            {
                services.AddAuthentication()
                .AddOpenIdConnect(options =>
                {
                    configuration.GetSection("OpenIDConnectSettings").Bind(options);
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.UsePkce = true;
                    options.CallbackPath = "/api/account/signin-oidc";

                    options.SkipUnrecognizedRequests = true;

                    options.Scope.Add("email");

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                    };

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUsuarioService>();

                            // Obtener las claims actuales del token validado
                            var claims = context.Principal.Claims.ToList();

                            var userName = claims.FirstOrDefault(c => c.Type == "nickname")!.Value!;
                            var name = claims.FirstOrDefault(c => c.Type == "name")!.Value;
                            var email = claims.FirstOrDefault(c => c.Type == ClaimValueTypes.Email)!.Value!;

                            var grupos = claims.Where(c => c.Type == "groups").Select(c => c.Value);

                            string url = $"{context.Request.Scheme}://{context.Request.Host.Value}";

                            var result = await userService.ProcesarIngresoUsuarioOAuth(new OAuthUserDTO()
                            {
                                Host = url,
                                IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                                Name = name,
                                UserName = userName,
                                Email = email,
                                Grupos = grupos,
                            });

                            if (!result.IsSuccess) throw new Exception("No es posible crear la sesión");


                            var session = result.Value;

                            claims.Add(new Claim("IdUsuario", session.IdUsuario.ToString()));
                            claims.Add(new Claim("IdSession", session.IdSession.ToString()));
                            claims.Add(new Claim("IdAuditoria", session.IdAuditoria.ToString()));
                            claims.Add(new Claim("RememberMe", session.RememberMe.ToString()));
                            claims.Add(new Claim("Url", url));
                            claims.Add(new Claim("TipoUsuario", "OAuth"));

                            // Si necesitas modificar el principal del usuario (añadir/modificar claims)
                            var identity = new ClaimsIdentity(claims, context.Principal.Identity.AuthenticationType);
                            context.Principal = new ClaimsPrincipal(identity);

                            // Aquí podrías ejecutar otras tareas asíncronas o síncronas necesarias antes de que se cree la cookie
                            // ...

                            await Task.CompletedTask; // Marca como completado el evento
                        }
                    };
                });
            }

            services.AddAuthorization();
            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthorizationService>();
            services.AddScoped<AuthService>();
            #endregion

            return services;
        }
        private static IServiceCollection ConfigForwardedHeaders(this IServiceCollection services, IConfiguration configuration)
        {
            //Forwarded headers config
            ForwardedHeadersConfig forwardedHeadersConfig = new();
            configuration.GetSection("ForwardedHeadersConfig").Bind(forwardedHeadersConfig);

            if (forwardedHeadersConfig.Enabled)
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit 
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();

                    foreach (var proxy in forwardedHeadersConfig.KnownProxies)
                    {
                        options.KnownProxies.Add(IPAddress.Parse(proxy));
                    }
                });
            }

            return services;
        }
        private static IServiceCollection AddRadzen(this IServiceCollection services)
        {
            services.AddScoped<TooltipService>();
            services.AddScoped<TooltipHelper>();
            services.AddRadzenComponents();

            return services;
        }
        private static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<EliminacionArchivosService>();
            services.AddHostedService<InactivacionUsuariosService>();

            return services;
        }
        private static IServiceCollection AddCircuitServices(this IServiceCollection services)
        {
            services.AddSingleton<ICircuitUserService, CircuitUserService>();
            services.AddScoped<CircuitHandler>((sp) => new CircuitHandlerService(sp.GetRequiredService<ICircuitUserService>()));

            return services;
        }
        private static IServiceCollection AddRecaptchaServices(this IServiceCollection services)
        {
            #region Recaptcha
            services.AddTransient<GooglereCaptchaService>();
            services.AddScoped(sp => new HttpClient());
            #endregion

            return services;
        }
        private static IServiceCollection ConfigBlazorServer(this IServiceCollection services)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents(options =>
                {
                    options.DetailedErrors = false;
                    options.DisconnectedCircuitMaxRetained = 100;
                    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
                    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
                    options.MaxBufferedUnacknowledgedRenderBatches = 10;
                });

            services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddHubOptions(options =>
                {
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                    options.EnableDetailedErrors = false;
                    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
                    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                    options.MaximumParallelInvocationsPerClient = 1;
                    options.MaximumReceiveMessageSize = 102400000;
                    options.StreamBufferCapacity = 10;
                });

            return services;
        }
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.ConfigBlazorServer();
            services.ConfigAuth(configuration);
            services.AddControllers();
            services.AddCircuitServices();
            services.AddScoped<ProtectedSessionStorage>();
            services.AddHostedServices();
            services.AddRadzen();
            services.AddRecaptchaServices();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(181);
            });

            services.ConfigForwardedHeaders(configuration);

            return services;
        }
    }
}
