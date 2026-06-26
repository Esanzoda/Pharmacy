using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IOrderRepository: IBaseRepository<Order> 
{
    
}
public class OrderRepository:BaseRepository<Order>,IOrderRepository
{
    public OrderRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Order?> GetByIdAsync(long id)
    {
        return await DbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

}