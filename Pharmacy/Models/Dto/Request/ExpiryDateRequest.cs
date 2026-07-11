using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class ExpiryDateRequest : BaseEntity
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
}