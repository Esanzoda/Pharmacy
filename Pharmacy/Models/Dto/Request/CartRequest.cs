namespace Pharmasy.Models.Dto.Request;

public class CartRequest
{
    public long CustomerId { get; set; }
    public List<CartItemRequest> CartItems { get; set; }
}