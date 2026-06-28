using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Request;

public class  OrderRequest
{
    
    public long UserId { get; set; }
    public OrderType OrderType { get; set; }
    public DateTime? GetTime { get; set; }
    public string?  Adress { get; set; }
    
  /*  public List<OrderItemRequest> OrderItems { get; set; }
        = new List<OrderItemRequest>();*/
}