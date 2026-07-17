using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public record UpdateOrderResponse
{
    public long UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}