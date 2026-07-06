using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Request;

public class ProductRequest
{
    public string Name { get; set; }
    public ProductType ProductType { get; set; }
    public long CategoryId { get; set; }
    public string Description { get; set; }
    public CountryEnum Country { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public string Barcode { get; set; }
    public int Stock { get; set; }
    public DateTime ExpiryDate { get; set; }
}