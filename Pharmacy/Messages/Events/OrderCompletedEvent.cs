using System.Reflection.Metadata.Ecma335;

namespace Pharmasy.Messages.Evants;

public class OrderCompletedEvent
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CompletedAt { get; set; }
}