using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class ProductResponse
{
    public long Id { get; set; }
    public string  Name { get; set; }
    public string  Description { get; set; }
    public int Stock { get; set; }
    public decimal OrderPrice { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    
}