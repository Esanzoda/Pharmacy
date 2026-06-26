using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem>
{
    Task<List<OrderItem>> GetAllOrderItems(long orderId); 
    
}
public class OrderItemRepository:BaseRepository<OrderItem>,IOrderItemRepository
{
    public OrderItemRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }


    public Task<List<OrderItem>> GetAllOrderItems(long orderId)
    {
        return DbContext.OrderItems
            .Where(x=>x.OrderId == orderId)
            .ToListAsync();
    }
}