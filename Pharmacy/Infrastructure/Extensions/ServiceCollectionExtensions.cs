using FluentValidation;
using FluentValidation.AspNetCore;
using Pharmacy.CQRS;
using Pharmacy.Data;
using Pharmacy.Interfaces;
using Pharmacy.Jobs;

namespace Pharmacy.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AuditableInterceptor>();
        serviceCollection.AddAuthorization();
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddFluentValidationAutoValidation();
        serviceCollection.AddValidatorsFromAssemblyContaining<Program>();
        serviceCollection.AddAutoMapper(
            _ => { },
            AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddScoped<IMessageService, MessageService>();
        serviceCollection.AddScoped<IApplicationDbContext, AppDbContext>();
        serviceCollection.AddScoped<CheckExpiredProductsJob>();
        serviceCollection.AddScoped<Report>();
        serviceCollection.AddOpenApi();
        serviceCollection.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
    }
}