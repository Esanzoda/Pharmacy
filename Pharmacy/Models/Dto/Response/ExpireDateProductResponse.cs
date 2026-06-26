using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class ExpireDateProductResponse
{
    public DateTime DateTime { get; set; }
    public int  Count { get; set; }
    public List<ExpireDateItems> ExpiredateItemsListResponse { get; set; }
}

