namespace Pharmasy.Models.Dto.Request;

public class OrderReservationRequest
{
    public long UserId { get; set; }


    public List<OrderItemRequest> OrderItemRequests { get; set; }
        = new List<OrderItemRequest>();
}