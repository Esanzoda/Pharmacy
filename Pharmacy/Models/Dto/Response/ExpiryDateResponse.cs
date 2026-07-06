namespace Pharmasy.Models.Dto.Response;

public class ExpiryDateResponse
{
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Count { get; set; }

    public List<ExpiryDateItemResponse> ExpiryDateItems { get; set; }
}

public class ExpiryDateItemResponse
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
}