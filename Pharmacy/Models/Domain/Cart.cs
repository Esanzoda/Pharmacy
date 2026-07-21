using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Domain;

public class Cart : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public decimal TotalAmount { get; set; }

    public List<CartItem?> CartItems { get; set; } = new List<CartItem?>();

}