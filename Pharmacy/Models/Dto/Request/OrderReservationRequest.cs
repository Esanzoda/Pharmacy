namespace Pharmasy.Models.Dto.Request;

public class OrderReservationRequest
{
    public long CustomerId { get; set; }
    public DateTime GetTime { get; set; }

    public List<OrderItemRequest> OrderItemRequests { get; set; }
        = new List<OrderItemRequest>();
}