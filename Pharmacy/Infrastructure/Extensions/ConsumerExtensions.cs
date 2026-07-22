using MassTransit;
using Pharmacy.Consumers;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Infrastructure.Extensions;

public static class ConsumerExtensions
{
    public static void AddConsumers(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        var rabbitmq = configuration.GetSection(RabbitMqOption.SettingName)
            .Get<RabbitMqOption>();
        if (rabbitmq != null)
        {
            serviceCollection.AddMassTransit(x =>
            {
                x.AddConsumer(typeof(OrderCreatedConsumer));
                x.AddConsumer(typeof(OrderCanselledConsumer));
                x.AddConsumer(typeof(OrderCompletedConsumer));
                x.AddConsumer(typeof(ReportToCeoOrderCompleted));
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmq.Host, rabbitmq?.VirtualHost, hostConfigure =>
                    {
                        if (rabbitmq != null)
                        {
                            hostConfigure.Username(rabbitmq.UserName);
                            hostConfigure.Password(rabbitmq.Password);
                        }
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}