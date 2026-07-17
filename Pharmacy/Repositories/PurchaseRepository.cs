using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IPurchaseRepository : IBaseRepository<Purchase>
{
    public Task<List<Purchase>> GetPurchaseByEmployeId(long employeId, int pageNumber, int pageSize);
    public Task<List<Purchase>> GetPurchaseByDate(DateTime date, int pageNumber, int pageSize);
}

public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
{
    public PurchaseRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Purchase?> GetByIdAsync(long id)
    {
        return await DbContext.Purchases
            .Include(o => o.PurchaseItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public override async Task<List<Purchase>> GetAllByPaginationAsync(int pageNumber, int pageSize,CancellationToken cancellationToken=default)
    {
        return await DbContext.Purchases
            .Include(o => o.PurchaseItems)
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Purchase>> GetPurchaseByEmployeId(long employeId, int pageNumber, int pageSize)
    {
        return await  DbContext.Purchases
            .Include(o => o.PurchaseItems)
            .Where(o => o.EmployeeId == employeId)
            .OrderBy(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Purchase>> GetPurchaseByDate(DateTime date, int pageNumber, int pageSize)
    {
        return await   DbContext.Purchases
            .Include(o => o.PurchaseItems)
            .Where(x=>x.CreatedAt == date)  
            .OrderBy(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}