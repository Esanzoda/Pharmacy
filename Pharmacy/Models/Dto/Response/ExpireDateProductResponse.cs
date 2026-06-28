using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class  ExpireDateProductResponse
{
    public long Id { get; set; }
    public DateTime DateTime { get; set; }
    public int  Count { get; set; }
    public decimal ToTalPrice { get; set; }
    public decimal TotalPurchase { get; set; }
    public List<ExpireDateItems> ExpiredateItemsListResponse { get; set; }
    
}

public class ExpireDateItemsResponse
{
    public long  Id { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    
}