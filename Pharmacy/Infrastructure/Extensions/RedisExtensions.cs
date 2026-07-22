namespace Pharmacy.Infrastructure.Extensions;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection serviceCollection, WebApplicationBuilder webApplicationBuilder)
    {
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = webApplicationBuilder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "Pharmacy";
            
        });
    }
}