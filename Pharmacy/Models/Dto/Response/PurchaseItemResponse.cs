namespace Pharmasy.Models.Dto.Response;

public class PurchaseItemResponse
{
    public long id { get; set; }
    public string Name { get; set; }
    public decimal PurchasePrice  { get; set; }
    public int  Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
}