using System.Runtime.Serialization;

namespace Pharmasy.Models.Dto.Response;

public class EmployeResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string  Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Salary { get; set; }
    
}