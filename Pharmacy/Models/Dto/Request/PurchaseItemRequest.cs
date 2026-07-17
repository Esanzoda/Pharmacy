namespace Pharmasy.Models.Dto.Request;

public record PurchaseItemRequest
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public string Barcode { get; set; }
    public DateTime ExpiryDate { get; set; }
}