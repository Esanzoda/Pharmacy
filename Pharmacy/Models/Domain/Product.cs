using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Product : BaseEntity
{
    public string  Name { get; set; }
    public string  Description { get; set; }
    public int Stock { get; set; }
    public string Barcode { get; set; } 
    public decimal PurchasePrice { get; set; }
    public decimal OrderPrice { get; set; }
    public ProductType ProductType { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
   public DateTime ExpiryDate { get; set; }
   
   
}