namespace Pharmacy.Models.Domain;

public class ExpiryDateItems : BaseEntity
{
    public long ExpiryDateProductId { get; set; }
    public ExpiryDateProduct ExpiryDateProduct { get; set; } = null!;
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
}