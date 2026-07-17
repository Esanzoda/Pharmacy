using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public record OrderItemRequest
{
    public long ProductId { get; set; }

    // public Product Product { get; set; }
    public int Quantity { get; set; }
}