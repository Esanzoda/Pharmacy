namespace Pharmacy.Infrastructure.Setting;

public class ConnectionStringsOption
{
    public static string SettingName { get; set; } = "ConnectionStrings";
    public string DefaultConnection { get; set; }=string.Empty;

    public string Redis { get; set; }=string.Empty;

    public string InstanceName { get; set; }=string.Empty;
    
    
}