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

public class PurchaseItemResponse
{
    public long id { get; set; }
    public long ProductId { get; set; }
    public string Name { get; set; }
    public decimal PurchasePrice  { get; set; }
    public int  Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
}