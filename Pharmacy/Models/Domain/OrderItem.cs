using System.Reflection.Metadata.Ecma335;

namespace Pharmacy.Models.Domain;

public class OrderItem : BaseEntity
{
    public long ProductId { get; set; }
    public long OrderId { get; set; }
    public Product? Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}