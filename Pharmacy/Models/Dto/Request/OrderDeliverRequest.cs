using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class OrderDeliverRequest
{
    
    public long CustomerId { get; set; }
    public string  Adress { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
}