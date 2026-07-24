namespace Pharmacy.Models.Dto.Response;

public record LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}