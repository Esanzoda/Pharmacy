namespace Pharmacy.Models.Domain;

public class Deliver : BaseEntity
{
    public long PharmacyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Shot { get; set; } = 0;
    public string PasswordHash { get; set; } = string.Empty;
    public List<Order> Orders { get; set; } = new List<Order>();
}