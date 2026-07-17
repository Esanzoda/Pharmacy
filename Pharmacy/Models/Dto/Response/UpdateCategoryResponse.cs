using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Response;

public class UpdateCategoryResponse
{
    public string?  Name { get; set; }
    public string? Description { get; set; }
    public CategoryStatus CategoryStatus { get; set; } 
}