using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Order:BaseEntity
{
    public decimal Totalprise { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus  OrderStatus { get; set; }
    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public long? EmployeId { get; set; }
    public Employee? Employee { get; set; }
   public List<OrderItem> OrderItems { get; set; }
   =new List<OrderItem>();
    
}