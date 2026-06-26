using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public class ProductResponse
{
    public long Id { get; set; }
    public string?  Name { get; set; }
    public string?  Description { get; set; }
    public long CategoryId { get; set; }
    public CountryEnum Country { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? Barcode { get; set; }
    public ProductType ProductType { get; set; }
    public decimal TotalAmout { get; set; }
    
    
}