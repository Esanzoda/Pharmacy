using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IPurchaseItemRepository : IBaseRepository<PurchaseItem>
{
    Task<PurchaseItem?> GetByBarcodeAsync(long purchaseId, string barcode);
}

public class PurchaseItemRepository : BaseRepository<PurchaseItem>, IPurchaseItemRepository
{
    public PurchaseItemRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<PurchaseItem?> GetByBarcodeAsync(long purchaseId, string barcode)
    {
        return await DbContext.PurchaseItems
            .FirstOrDefaultAsync(x => x.Barcode == barcode && x.PurchaseId == purchaseId);
    }
}