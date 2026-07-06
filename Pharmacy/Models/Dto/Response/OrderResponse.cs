using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class OrderResponse
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public decimal TotalAmout { get; set; }
    public DateTime CreateAt { get; set; }
    public List<OrderItemResponse> OrderItemResponses { get; set; }
}