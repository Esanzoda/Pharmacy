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
    public decimal OrderPrice { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    
}