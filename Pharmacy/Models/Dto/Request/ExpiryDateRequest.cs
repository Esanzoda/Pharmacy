using Pharmacy.Models.Domain;

namespace Pharmacy.Models.Dto.Request;

public class ExpiryDateRequest : BaseEntity
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
}