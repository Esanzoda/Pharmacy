using Pharmasy.Models.Domain;

namespace Pharmasy.Models.Dto.Response;

public class CustomerResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string  Email { get; set; }
    public string Adsress { get; set; }
    public string PhoneNumber { get; set; }
}