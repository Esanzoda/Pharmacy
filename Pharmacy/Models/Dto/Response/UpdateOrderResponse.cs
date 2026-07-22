using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Response;

public record UpdateOrderResponse
{
    public long UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}