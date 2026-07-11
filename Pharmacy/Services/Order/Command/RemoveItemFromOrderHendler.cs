using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;
public record RemoveItemFromOrderCommand(long OrderId, long ItemId): IRequest<OrderResponse>;
public class RemoveItemFromOrderHendler:OrderDiBase,IRequestHandler<RemoveItemFromOrderCommand,OrderResponse>
{
    public RemoveItemFromOrderHendler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository)
        : base(orderRepository, orderItemRepository, productRepository)
    {
    }

    public async Task<OrderResponse> Handle(RemoveItemFromOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new ResourseNotFoundException("OrderItem not found");
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

        var product = await ProductRepository.GetByIdAsync(itemToRemove.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundException($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;
        await ProductRepository.UpdateAsync(product);
        await ProductRepository.SaveChangesAsync();

        order.OrderItems.Remove(itemToRemove);
        order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        await OrderRepository.UpdateAsync(order);
        await OrderRepository.SaveChangesAsync();

        await OrderItemRepository.DeleteAsync(itemToRemove.Id);
        await OrderItemRepository.SaveChangesAsync();

        return Mapper.Map<OrderResponse>(order);
    }
}