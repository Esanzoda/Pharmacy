namespace Pharmacy.Models.Domain;

public class CartItem : BaseEntity
{
    public long CustomerId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public Cart? Cart { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}