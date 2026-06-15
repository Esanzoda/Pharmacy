namespace Pharmasy.Models.Dto.Request;

public class PurchaseItemRequest
{
    public long PurchaseId { get; set; }
    public long ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal PurchasePrise { get; set; }
    public decimal OrderPrise { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    
}