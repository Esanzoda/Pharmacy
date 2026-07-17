namespace Pharmasy.Models.Dto.Request;

public record CartRequest
{
    public long CustomerId { get; set; }
    public List<CartItemRequest> CartItems { get; set; }
}