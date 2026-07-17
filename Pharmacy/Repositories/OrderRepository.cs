using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetOrdersByOrderStatusAndDayAsync(OrderStatus status, DateTime day);
    Task<List<Order>> GetOrdersByOrderStatusAsync(OrderStatus status,int pageNumber, int pageSize);
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

    public override async Task<List<Order>> GetAllByPaginationAsync(int pageNumber, int pageSize,CancellationToken cancellationToken=default)
    {
        return await DbContext.Orders
            .Include(o => o.OrderItems)
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

//for check job , for raport to ceo
    public async Task<List<Order>> GetOrdersByOrderStatusAndDayAsync(OrderStatus status, DateTime day)
    {
        return await DbContext.Orders
            .Where(o => o.OrderStatus == status && o.CreatedAt == day)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByOrderStatusAsync(OrderStatus status, int pageNumber, int pageSize)
    {
        return await DbContext.Orders
            .Where(o => o.OrderStatus == status)
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
}