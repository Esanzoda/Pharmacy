namespace Pharmacy.Models.Dto.Response;

public record CustomerResponse
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}