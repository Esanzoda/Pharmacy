namespace Pharmacy.Models.Dto.Request;

public record CreateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
}