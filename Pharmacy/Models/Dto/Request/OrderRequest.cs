using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class  OrderRequest
{
    
    public long CustomerId { get; set; }
    public DateTime? GetTime { get; set; }
    public string?  Adress { get; set; }
    
    public List<OrderItemRequest> OrderItems { get; set; }
        = new List<OrderItemRequest>();
}