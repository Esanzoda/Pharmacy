namespace Pharmasy.Models.Dto.Response;

public class LoginResponse
{
    public string AccessToken{ get; init; }=string.Empty;
    public string RefreshToken{ get; init; }=string.Empty;
}