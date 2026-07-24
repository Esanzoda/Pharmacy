namespace Pharmacy.Models.Dto.Request;

public record CartItemRequest
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}