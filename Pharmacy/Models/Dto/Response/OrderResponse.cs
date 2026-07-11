using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class OrderResponse
{
    public long Id { get; set; }
    public long? CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemResponse> OrderItemResponses { get; set; }
}