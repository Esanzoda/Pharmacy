using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Employee:BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public Role? Role { get; set; }
    public string PaswordHash { get; set; }
    
}