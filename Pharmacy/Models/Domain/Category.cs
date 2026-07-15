using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Category : BaseEntity
{
    public string Name { get; set; }=string.Empty;
    public string Description { get; set; }=string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<Product> Products { get; set; }
}