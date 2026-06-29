using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    
    public decimal? Salary { get; set; }
    public Cart? Cart { get; set; }
    public List<Order> Orders { get; set; } = new();
}