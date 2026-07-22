namespace Pharmacy.Models.Dto.Response;

public record CategoryResponse
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}