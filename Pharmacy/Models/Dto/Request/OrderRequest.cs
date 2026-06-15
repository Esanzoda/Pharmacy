using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Request;

public class OrderRequest
{
    public long CustomerId { get; set; }
    public CustomerRequest? Customer { get; set; }
    public long EmployeeId { get; set; }
    public EmployeRequest Employe { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; }
}