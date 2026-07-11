namespace Pharmasy.Messages.Evants;

public class OrderCancelledEvent
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public DateTime UpdateTime { get; set; }
}