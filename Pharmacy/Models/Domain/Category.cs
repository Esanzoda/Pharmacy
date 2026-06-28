using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Domain;

public class Category:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public CategoryStatus CategoryStatus { get; set; }
    public List<Product>Products { get; set; }
    
} 