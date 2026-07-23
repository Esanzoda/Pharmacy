using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Dto.Request;

public record UpdateCategoryRequest
{
    public UpdateCategoryRequest(string name, string description, CategoryStatus categoryStatus)
    {
        Name = name;
        Description = description;
        CategoryStatus = categoryStatus;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public CategoryStatus CategoryStatus { get; set; }
}