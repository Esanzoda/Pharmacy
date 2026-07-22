namespace Pharmacy.Infrastructure.Setting;

public class JwtOption
{
    public static string SettingName { get; set; } = "JwtSetting";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; }
}