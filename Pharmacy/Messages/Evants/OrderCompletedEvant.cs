using System.Reflection.Metadata.Ecma335;

namespace Pharmasy.Messages.Evants;

public class OrderCompletedEvant
{
    public long OrderId { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmout { get; set; }
    public DateTime CompletedAt { get; set; }
}