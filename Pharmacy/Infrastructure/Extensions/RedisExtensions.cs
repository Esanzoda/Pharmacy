using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Infrastructure.Extensions;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString.Redis;
            options.InstanceName = connectionString.InstanceName;
        });
    }
}