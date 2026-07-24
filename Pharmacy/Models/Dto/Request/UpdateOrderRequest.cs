using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record UpdateOrderRequest
{
    public long UserId { get; init; }
    public OrderStatus OrderStatus { get; init; }
}