using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Customer : BaseEntity
{
    public string? Name { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public Cart? Cart { get; set; }
    public List<Order> Orders { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}