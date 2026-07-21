namespace Pharmasy.Messages.Events;

public class OrderCreatedEvent
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal DelivePrice { get; set; }
    public decimal TotalAmount { get; set; }
    
}