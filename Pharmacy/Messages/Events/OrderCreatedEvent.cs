namespace Pharmacy.Messages.Events;

public class OrderCreatedEvent
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal DeliverPrice { get; set; }
    public decimal TotalAmount { get; set; }
    
}