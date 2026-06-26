namespace Pharmasy.Models.Dto.Request;

public class CartRequest
{
    public List<CartItemRequest> CartItems { get; set; }
}
public class CartItemRequest
{
    public long CartId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    
}