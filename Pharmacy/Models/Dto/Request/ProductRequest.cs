using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record ProductRequest
{
    public string? Name { get; set; }
    public ProductType ProductType { get; set; }
    public long CategoryId { get; set; }
    public string? Description { get; set; }
    public CountryEnum Country { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public string? Barcode { get; set; }
    public int Stock { get; set; }
    public DateTime ExpiryDate { get; set; }
}