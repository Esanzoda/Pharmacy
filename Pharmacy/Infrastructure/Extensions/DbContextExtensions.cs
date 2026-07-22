using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Infrastructure.Extensions;

public static class DbContextExtensions
{

    public static void AddAppDbContext(this IServiceCollection serviceCollection ,IConfiguration configuration)
    {
        var connectionString=configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
       serviceCollection.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString.DefaultConnection)
                .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
        });
     
    }
    
}