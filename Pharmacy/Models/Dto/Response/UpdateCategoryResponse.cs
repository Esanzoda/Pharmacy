using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Response;

public class UpdateCategoryResponse
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
}