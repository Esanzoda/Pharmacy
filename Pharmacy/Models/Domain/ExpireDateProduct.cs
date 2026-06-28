
namespace Pharmasy.Models.Domain;

public class ExpireDateProduct:BaseEntity
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }

    public List<ExpireDateItems> ExpiredateItemsList { get; set; } =
        new List<ExpireDateItems>();

}
public class ExpireDateItems:BaseEntity
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
    
    
} 