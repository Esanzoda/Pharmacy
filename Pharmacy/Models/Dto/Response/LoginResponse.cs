namespace Pharmasy.Models.Dto.Response;

public record LoginResponse
{
    public string AccessToken{ get; init; }=string.Empty;
    public string RefreshToken{ get; init; }=string.Empty;
}