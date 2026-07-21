namespace Pharmasy.Models.Domain;

public class ExpiryDateProduct : BaseEntity
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }

    public List<ExpireDateItems> ExpiredateItemsList { get; set; } =
        new List<ExpireDateItems>();
}

public class ExpireDateItems : BaseEntity
{
    public long ExiryDateProductId { get; set; }
    public ExpiryDateProduct ExpiryDateProduct { get; set; } = null!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = null;
    public string ProductName { get; set; } = string.Empty; 
    public int Quantity { get; set; }   
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
}