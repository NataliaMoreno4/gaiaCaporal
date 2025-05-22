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


                await next();
            });

            return app;
        }
    }
}
