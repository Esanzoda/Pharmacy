namespace Pharmasy.Models.Domain;

public class Purchase:BaseEntity
{
    public Supplier Supplier { get; set; }
    public long SuppilerId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<PurchaseItem> PurchaseItems { get; set; }
}