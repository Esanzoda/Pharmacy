using System.Runtime.Serialization;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public record EmployeeResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Salary { get; set; }
    public Role Role { get; set; }
}