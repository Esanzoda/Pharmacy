using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class OrderItemRequest
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
   // public Product Product { get; set; }
    public int Quantity { get; set; }
    
}