namespace Pharmasy.Messages.Evants;

public class OrderCompletedEvant
{
    public long OrderId { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmout { get; set; }  
}