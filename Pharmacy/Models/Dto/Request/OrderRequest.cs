using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record OrderRequest
{
    public OrderType OrderType { get; init; }
    public DateTime? GetTime { get; set; }
    public string? Adress { get; init; }

    public List<OrderItemRequest> OrderItems { get; set; }
}