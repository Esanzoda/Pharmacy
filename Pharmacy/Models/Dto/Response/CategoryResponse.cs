namespace Pharmacy.Models.Dto.Response;

public record CategoryResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
}