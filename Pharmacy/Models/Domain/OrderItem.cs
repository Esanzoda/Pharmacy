namespace Pharmacy.Models.Domain;

public class OrderItem : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ProductId { get; set; }
    public long OrderId { get; set; }
    public Product Product { get; set; }= null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}