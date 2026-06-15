using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IPurchaseItemRepository : IBaseRepository<PurchaseItem>
{
    
}
public class PurchaseItemRepository:BaseRepository<PurchaseItem>, IPurchaseItemRepository
{
    public PurchaseItemRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }
}