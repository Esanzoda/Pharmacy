namespace Pharmasy.Models.Domain;

public class Purchase:BaseEntity
{
    public decimal TotalPrice { get; set; }

    public List<PurchaseItem> PurchaseItems { get; set; }
        = new List<PurchaseItem>();
}