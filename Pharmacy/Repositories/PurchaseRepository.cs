using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IPurchaseRepository: IBaseRepository<Purchase>
{
    
}
public class PurchaseRepository:BaseRepository<Purchase>,IPurchaseRepository
{
    public PurchaseRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    public override async Task<Purchase?> GetByIdAsync(long id)
    {
        return await _dbContext.Purchases
            .Include(o => o.PurchaseItems)
            .FirstOrDefaultAsync(o => o.Id == id);

    }
}