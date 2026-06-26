namespace Pharmasy.Messages.Evants;

public class OrderCancelledEvant
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public DateTime UpdateTime { get; set; }
}