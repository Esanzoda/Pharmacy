using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ISupplierRepository: IBaseRepository<Deliver>
{
    
}
public class  SupplierRepository:BaseRepository<Deliver>,ISupplierRepository
{
    public SupplierRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}