namespace Pharmasy.Models.Domain;

public class Purchase:BaseEntity
{
    public decimal TotalAmout { get; set; }

    public List<PurchaseItem> PurchaseItems { get; set; }
        = new List<PurchaseItem>();
}