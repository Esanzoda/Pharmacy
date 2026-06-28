namespace Pharmasy.Messages.Evants;

public class OrderCreatedEvant
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public decimal TotalAmout { get; set; }
} 