namespace Pharmacy.Models.Domain;

public class Purchase : BaseEntity
{
    public decimal TotalAmount { get; set; }
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public List<PurchaseItem> PurchaseItems { get; set; }
        = new List<PurchaseItem>();
}