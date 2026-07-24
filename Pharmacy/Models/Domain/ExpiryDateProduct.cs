namespace Pharmacy.Models.Domain;

public class ExpiryDateProduct : BaseEntity
{
    
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }

    public List<ExpiryDateItems> ExpiryDateItemsList { get; set; } =
        new List<ExpiryDateItems>();
}