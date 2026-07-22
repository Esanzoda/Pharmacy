using System.Reflection.Metadata.Ecma335;

namespace Pharmacy.Infrastructure.Setting;

public class RabbitMqOption
{
    public static string SettingName { get; set; } = "Rabbitmq";
    public string Host { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}