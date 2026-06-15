using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ICartRepository : IBaseRepository<Cart> 
{
    
}
public class CartRepository:BaseRepository<Cart>,ICartRepository
{
    public CartRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}