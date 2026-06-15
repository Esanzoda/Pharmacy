namespace Pharmasy.Models.Dto.Response;

public class OrderItemResponse
{
    public long OrderId { get; set; }
    public long MedicineId { get; set; }
    public int Quantity { get; set; }
    public decimal OrderPrice { get; set; }
    
}