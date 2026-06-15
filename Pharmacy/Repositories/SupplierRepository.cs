using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ISupplierRepository: IBaseRepository<Supplier>
{
    
}
public class SupplierRepository:BaseRepository<Supplier>,ISupplierRepository
{
    public SupplierRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}