namespace Pharmasy.Models.Dto.Request;

public class CartRequest
{
    public long CistomerId { get; set; }
    public List<CartItemRequest> CartItems { get; set; }
}