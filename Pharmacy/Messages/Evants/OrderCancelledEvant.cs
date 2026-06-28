namespace Pharmasy.Messages.Evants;

public class OrderCancelledEvant
{
    public long OrderId { get; set; }
    public long UserId { get; set; }
    public DateTime UpdateTime { get; set; }
} 