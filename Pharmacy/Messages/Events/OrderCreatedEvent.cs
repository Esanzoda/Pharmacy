using System.Runtime.InteropServices.JavaScript;

namespace Pharmasy.Messages.Evants;

public class OrderCreatedEvent
{
    public long OrderId { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
}