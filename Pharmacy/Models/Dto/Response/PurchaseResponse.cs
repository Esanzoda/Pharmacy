using Pharmacy.Models.Dto.Request;

namespace Pharmacy.Models.Dto.Response;

public class PurchaseResponse
{
    public long Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PurchaseItemResponse> PurchaseItems { get; set; }
}