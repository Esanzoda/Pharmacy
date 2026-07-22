using Hangfire;
using Hangfire.PostgreSql;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Infrastructure.Extensions;

public static class HangfireExtensions
{
    public static void AddHangfire(this IServiceCollection serviceCollection,IConfiguration configuration)
         {
             var connectionString=configuration.GetSection(ConnectionStringsOption.SettingName)
                 .Get<ConnectionStringsOption>()!;
        serviceCollection.AddHangfire((_, cfg) =>
        {
            cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(connectionString.DefaultConnection);
                });
        }).AddHangfireServer();
    }
}