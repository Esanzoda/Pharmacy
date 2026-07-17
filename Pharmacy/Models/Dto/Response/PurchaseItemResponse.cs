namespace Pharmasy.Models.Dto.Response;

public record PurchaseItemResponse
{
    public long id { get; set; }
    public long ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public int Quantity { get; set; }
    public string Barcode { get; set; }
    public decimal TotalPrice { get; set; }
}