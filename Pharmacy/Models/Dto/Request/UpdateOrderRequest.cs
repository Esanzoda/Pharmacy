using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public class UpdateOrderRequest
{
    public long UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}