using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PlantillaBlazor.Persistence;
using PlantillaBlazor.Services;
using PlantillaBlazor.Web;
using PlantillaBlazor.Web.Components;
using Serilog;


//Logs
var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

Log.Information("Starting up the application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
    });

    //Serilog
    builder.Host.UseSerilog();

    #region Capas
    //Persistence
    builder.Services.AddPersistence(builder.Configuration);
    //Services
    builder.Services.AddServices(builder.Configuration);
    //Presentation
    builder.Services.AddPresentation(builder.Configuration);
    #endregion

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    app.UseResponseCompression();

    #region DB Migration
    app.ExecuteDBMigration();
    #endregion

    #region Auditoría en JSRuntime
    app.SetupAuditoriaJSRuntime();
    #endregion

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseHangfireDashboard("/hangfire");
    }

    app.UseStaticFiles();

    //Forwarded headers config
    app.SetupForwardedHeadersConfig(builder.Configuration);

    app.SetupCSPHeader();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    app.MapControllers();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.UseStatusCodePagesWithRedirects("/404");

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.Run();
}
catch (Exception exe)
{
    Log.Fatal(exe, "There was a problem starting the application");
    return;
}
finally
{
    Log.CloseAndFlush();
}