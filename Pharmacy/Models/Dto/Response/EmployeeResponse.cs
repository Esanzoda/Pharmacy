using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Response;

public record EmployeeResponse
{
    public long PharmacyId { get; set; }
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public decimal Salary { get; set; }
    public Role Role { get; set; }
}