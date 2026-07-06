using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class ExpiryDateRequrst : BaseEntity
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
}