using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Response;

public record ProductResponse
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public CountryEnum Country { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public required string Barcode { get; set; }
    public ProductType ProductType { get; set; }
}