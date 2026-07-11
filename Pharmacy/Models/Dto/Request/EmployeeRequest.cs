using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Request;

public class EmployeeRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public decimal Salary { get; set; }
    public Role Role { get; set; }
}