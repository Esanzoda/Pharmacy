namespace Pharmacy.Models.Dto.Request;

public record UpdateCustomerRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Address { get; init; }
}