using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetAllByOrderStatusAsync(OrderStatus status, DateTime day);
}

public class OrderRepository : BaseRepository<Order>, IOrderRepository
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

    public override async Task<List<Order>> GetAllByPagenationAsync(int pageNumber, int pageSize)
    {
        return await DbContext.Orders
            .Include(o => o.OrderItems)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllByOrderStatusAsync(OrderStatus status, DateTime day)
    {
        return await DbContext.Orders
            .Where(o => o.OrderStatus == status && o.CreatedAt >= day)
            .ToListAsync();
    }
}