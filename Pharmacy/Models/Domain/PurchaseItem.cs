namespace Pharmasy.Models.Domain;

public class PurchaseItem : BaseEntity
{
    public long PurchaseId { get; set; }
    public long ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public string Barcode { get; set; }

    // public string Name { get; set; } must update EF core if use it
    public decimal TotalPrice { get; set; }
    public DateTime ExpiryDate { get; set; }
}