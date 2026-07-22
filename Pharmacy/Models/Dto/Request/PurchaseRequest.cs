namespace Pharmacy.Models.Dto.Request;

public record PurchaseRequest
{
    public long EmployeeId { get; set; }
    public List<PurchaseItemRequest> PurchaseItems { get; set; }
}