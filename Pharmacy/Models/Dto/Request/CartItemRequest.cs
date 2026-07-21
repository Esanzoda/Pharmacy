namespace Pharmasy.Models.Dto.Request;

public record CartItemRequest
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
