using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class PurchaseRequest
{
    public long EmployeeId { get; set; }
    public List<PurchaseItemRequest> PurchaseItems { get; set; }
}