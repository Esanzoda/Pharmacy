using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Domain;

namespace Pharmacy.Models.Dto.Request;

public record OrderRequest
{
    public OrderType OrderType { get; set; }
    public DateTime? GetTime { get; set; }
    public string? Adress { get; set; }

    public List<OrderItemRequest> OrderItems { get; set; }
}