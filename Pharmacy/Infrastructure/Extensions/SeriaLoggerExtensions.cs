using Serilog;

namespace Pharmacy.Infrastructure.Extensions;

public static class SeriaLoggerExtensions
{
    public static void AddSeriaLogger(this WebApplicationBuilder webApplicationBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(webApplicationBuilder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console()
            .WriteTo.File("logs/pharmacy-log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}