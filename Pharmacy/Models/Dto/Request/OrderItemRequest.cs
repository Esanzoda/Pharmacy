namespace Pharmasy.Models.Dto.Request;

public class OrderItemRequest
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    
}