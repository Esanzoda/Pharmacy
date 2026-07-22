using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Response;

public class UpdateCategoryResponse
{
    public string?  Name { get; set; }
    public string? Description { get; set; }
    public CategoryStatus CategoryStatus { get; set; } 
}