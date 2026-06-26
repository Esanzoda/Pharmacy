using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Models.Dto.Response;

public class PurchaseResponse
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public SupplierResponse? Suppiler { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreateAt { get; set; }
    public List<PurchaseItemResponse> PurchaseItemResponses { get; set; }
    
}