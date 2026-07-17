using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public record UpdateOrderRequest
{
    public long UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}