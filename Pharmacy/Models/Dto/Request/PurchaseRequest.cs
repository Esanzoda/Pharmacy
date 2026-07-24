namespace Pharmacy.Models.Dto.Request;

public record PurchaseRequest
{
    public long EmployeeId { get; init; }
    public List<PurchaseItemRequest> PurchaseItems { get; set; }
}