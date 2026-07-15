namespace Pharmasy.Models.Dto.Response;

public class DeliverResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public decimal Shot{get; set;}
}