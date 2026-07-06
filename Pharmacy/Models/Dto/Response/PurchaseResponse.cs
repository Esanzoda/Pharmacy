using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Models.Dto.Response;

public class PurchaseResponse
{
    public long Id { get; set; }
    public decimal TotalAmout { get; set; }
    public DateTime CreateAt { get; set; }
    public List<PurchaseItemResponse> PurchaseItems { get; set; }
}