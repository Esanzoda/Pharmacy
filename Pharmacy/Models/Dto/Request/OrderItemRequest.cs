namespace Pharmacy.Models.Dto.Request;

public record OrderItemRequest
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}