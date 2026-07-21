using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Order.Commands;

public record RemoveItemFromOrderCommand(
    long CustomerId,
    long OrderId,
    long ItemId) : IRequest<OrderResponse>;

public class RemoveItemFromOrderHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<RemoveItemFromOrderCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(RemoveItemFromOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
        if (order == null)
        {
            throw new ResourseNotFoundException("Order not found");
        }

        if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessException("Can't remove item completed or cancelled order ");
        }

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == request.ItemId);
        if (itemToRemove == null)
        {
            throw new ResourseNotFoundException($"OrderItem not found");
        }

        var product = await dbContext.Products
            .FindAsync(itemToRemove.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ResourseNotFoundException($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;


        order.OrderItems.Remove(itemToRemove);
        order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);

        dbContext.OrderItems
            .Remove(itemToRemove);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrderResponse>(order);
    }
}