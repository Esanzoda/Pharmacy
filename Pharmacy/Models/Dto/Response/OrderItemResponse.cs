namespace Pharmasy.Models.Dto.Response;

public class OrderItemResponse
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal ProductOrderPrice { get; set; }
    
}