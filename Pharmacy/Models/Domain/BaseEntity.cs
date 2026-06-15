namespace Pharmasy.Models.Domain;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
}