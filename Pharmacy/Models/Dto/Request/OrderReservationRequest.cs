namespace Pharmasy.Models.Dto.Request;

public record OrderReservationRequest
{
    public long CustomerId { get; set; }


    public List<OrderItemRequest> OrderItemRequests { get; set; }
        = new List<OrderItemRequest>();
}