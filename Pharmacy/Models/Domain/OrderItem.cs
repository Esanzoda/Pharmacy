using System.Reflection.Metadata.Ecma335;

namespace Pharmasy.Models.Domain;

public class OrderItem:BaseEntity
{
    public long ProductId { get; set; }
    public Product Product { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; }
    public decimal Price { get; set; }
    public int  Quantity { get; set; }
    public decimal TotalPrice { get; set; }
} 