namespace Pharmacy.Models.Dto.Response;

public record DeliverResponse
{
    public long PharmacyId { get; set; }
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string Email { get; set; }
    public decimal Shot { get; set; }
}