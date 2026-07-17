namespace Pharmasy.Messages.Events;

public class OrderCompletedEvent
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CompletedAt { get; set; }
}