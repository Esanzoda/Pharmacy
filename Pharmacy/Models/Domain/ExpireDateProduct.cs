using Microsoft.EntityFrameworkCore.Storage;
using Pharmasy.Models.Dto.Response;

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
    public int Quantity { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
    
    
}