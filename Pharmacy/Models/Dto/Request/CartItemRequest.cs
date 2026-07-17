namespace Pharmasy.Models.Dto.Request;

public record CartItemRequest
{
    public long CustomerId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}