namespace Pharmasy.Models.Domain;

public class PurchaseItem:BaseEntity
{
    public long PurchaseId { get; set; }
    public long ProductId { get; set; } 
    public decimal PurchasePrice { get; set; }
    public decimal OrderPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    
} 