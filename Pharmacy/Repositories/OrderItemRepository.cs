using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem>
{
    
}
public class OrderItemRepository:BaseRepository<OrderItem>,IOrderItemRepository
{
    public OrderItemRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }
}