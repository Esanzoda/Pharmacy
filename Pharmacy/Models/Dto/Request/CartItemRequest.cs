namespace Pharmasy.Models.Dto.Request;

public class CartItemRequest
{
    public long CustomerId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}