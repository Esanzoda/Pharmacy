namespace Pharmasy.Models.Dto.Request;

public class  PurchaseItemRequest
{
    public long PurchaseId { get; set; }
    public long ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal TotalPrice { get; set; }
    
    
}