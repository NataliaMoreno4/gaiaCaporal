using Microsoft.EntityFrameworkCore;
using PlantillaBlazor.Domain.Common;
using PlantillaBlazor.Persistence.Data;
using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Web.Helpers;
using Serilog;

namespace PlantillaBlazor.Web
{
    public static class WebApplicationExtensionsMethod
    {
        public static WebApplication ExecuteDBMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

                var context = contextFactory.CreateDbContext();

                Log.Information("Iniciando migración de base de datos");

                var pendingMigrations = context.Database.GetPendingMigrations();

                Log.Information($"Migraciones pendientes por aplicar: {string.Join(",\n", pendingMigrations)}");
                Log.Information($"Tiene cambios en modelos pendientes: {context.Database.HasPendingModelChanges()}");

                context.Database.Migrate();

                var appliedMigrations = context.Database.GetAppliedMigrations(); ;
                Log.Information($"Migraciones aplicadas a la fecha: {string.Join(",\n", appliedMigrations)}");

                Log.Information("Fin de migración de base de datos");
            }

            return app;
        }
        public static WebApplication SetupAuditoriaJSRuntime(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var auditoriaService = scope.ServiceProvider.GetRequiredService<IAuditoriaService>();

                IJSRuntimeExtension.setAuditoriaService(auditoriaService);
            }

            return app;
        }
        public static WebApplication SetupForwardedHeadersConfig(this WebApplication app, IConfiguration configuration)
        {
            ForwardedHeadersConfig forwardedHeadersConfig = new();
            configuration.GetSection("ForwardedHeadersConfig").Bind(forwardedHeadersConfig);

            if (forwardedHeadersConfig.Enabled)
            {
                app.UseForwardedHeaders();
            }

            return app;
        }
        public static WebApplication SetupCSPHeader(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                //context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add("Referrer-Policy", "same-origin");
                //context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                //context.Response.Headers.Add("SameSite", "Strict");

                //context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
                //context.Response.Headers.Add("Expires", "0");
                //context.Response.Headers.Add("Pragma", "no-cache");


                #region CSP

                string finalCsp = "";

                string path = context.Request.Path.Value;

                if (path.Equals("/") || path.Equals("/login") || path.Equals("/ReestablecerContraseña") || path.Equals("/404"))
                {
                    //CSP para scanners

                    /*
                     Para los scanners se habilita un csp para aquellas url que son publicas
                    En este csp se aplican las directivas como default-src 'none' o en style-src se quita la
                    opción de unsafe-line.

                    Si se llegaran a aplicar estas directivas en todo el aplicativo, es probable que deje de funcionar
                     */

                    finalCsp = @"default-src 'none'; font-src 'self' fonts.gstatic.com unicons.iconscout.com cdnjs.cloudflare.com data:; img-src 'self' data:; script-src 'self' www.google.com www.gstatic.com www.googletagmanager.com; style-src 'self' https://unicons.iconscout.com https://fonts.googleapis.com cdnjs.cloudflare.com stackpath.bootstrapcdn.com https://www.googletagmanager.com; connect-src 'self' https://unicons.iconscout.com https://fonts.googleapis.com https://cdn.lordicon.com cdnjs.cloudflare.com fonts.gstatic.com www.google.com www.gstatic.com www.googletagmanager.com www.google-analytics.com stackpath.bootstrapcdn.com;frame-ancestors 'none';form-action 'self'; frame-src https://www.google.com www.gstatic.com; base-uri 'self';object-src 'self';manifest-src 'self'";
                }
                else
                {
                    finalCsp = @"default-src 'none'; font-src 'self' fonts.gstatic.com unicons.iconscout.com cdnjs.cloudflare.com data:; img-src 'self' data:; script-src 'self' www.google.com www.gstatic.com www.googletagmanager.com; style-src 'self' 'unsafe-inline' https://unicons.iconscout.com https://fonts.googleapis.com cdnjs.cloudflare.com stackpath.bootstrapcdn.com https://www.googletagmanager.com; connect-src 'self' https://unicons.iconscout.com https://fonts.googleapis.com https://cdn.lordicon.com cdnjs.cloudflare.com fonts.gstatic.com www.google.com www.gstatic.com www.googletagmanager.com www.google-analytics.com stackpath.bootstrapcdn.com;frame-ancestors 'none';form-action 'self'; frame-src https://www.google.com www.gstatic.com; base-uri 'self';object-src 'self';manifest-src 'self'";
                }

                context.Response.Headers.Add("Content-Security-Policy", finalCsp);

                #endregion

                await next();
            });

            return app;
        }
    }
}
