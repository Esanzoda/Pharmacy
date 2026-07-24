using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record CustomerRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public required string Address { get; init; }
    public Role Role { get; init; } = Role.Customer;
}