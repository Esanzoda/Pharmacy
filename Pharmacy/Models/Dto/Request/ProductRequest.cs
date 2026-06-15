using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Request;

public class ProductRequest
{
    public string Name { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal OrderPrice { get; set; }
    public int Stock { get; set; }
    public ProductType ProductType { get; set; }
    public DateTime ExpiryDate { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
    
    
}