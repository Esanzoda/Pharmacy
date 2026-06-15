using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class PurchaseRequest
{
    public long SuppilerId { get; set; }
    public SupplierRequest? Suppiler { get; set; }
    public long  EmployeId { get; set; }
    public Employee Employee { get; set; }
    public List<PurchaseItemRequest> PurchaseItemRequests { get; set; }
}