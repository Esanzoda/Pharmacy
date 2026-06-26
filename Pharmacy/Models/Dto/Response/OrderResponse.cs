using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class OrderResponse
{
    public long Id { get; set; }
    public decimal  TotalPrice { get; set; }
    public DateTime CreateAt { get; set; }
    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderItemResponse> OrderItemResponses { get; set; }
    
    
}