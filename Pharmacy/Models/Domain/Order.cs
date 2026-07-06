using System.Reflection.Metadata.Ecma335;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Order : BaseEntity
{
    public long CustomererId { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? Adress { get; set; }
    public DateTime? GetTime { get; set; }
    public decimal TotalAmout { get; set; }

    public List<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}