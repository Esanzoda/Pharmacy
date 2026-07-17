namespace Pharmasy.Models.Dto.Request;

public record CreateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}