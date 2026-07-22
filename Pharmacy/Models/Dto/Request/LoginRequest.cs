namespace Pharmacy.Models.Dto.Request;

public record LoginRequest
{
    public string Email { get; set; }

    public string Password { get; set; } 
}