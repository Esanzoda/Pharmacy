namespace Pharmasy.Models.Dto.Request;

public class OrderOnlineRequest
{
    
    public long CustomerId { get; set; }
    public CustomerRequest? Customer { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
}