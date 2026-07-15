using AutoMapper;
using MassTransit;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Messages.Evants;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;

public record UpdateOrderStatusCommand(long OrderId, UpdateOrderRequest Request) : IRequest<OrderResponse>;

public class UpdateOrderStatusHendler : OrderDiBase, IRequestHandler<UpdateOrderStatusCommand, OrderResponse>
{
    public UpdateOrderStatusHendler(ICustomerRepository customerRepository, IOrderRepository orderRepository,
        IProductRepository productRepository, IOrderItemRepository orderItemRepository, IMapper mapper,
        IPublishEndpoint publishEndpoint) : base(customerRepository, orderRepository, productRepository,
        orderItemRepository, mapper, publishEndpoint)
    {
    }

    public async Task<OrderResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new ResourseNotFoundException($"Order not found");
        }

//its order.status alredy changed
        if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Shipped ||
            order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessException($"Cannot update a {order.OrderStatus} order");
        }
//its new request will chanch ordersTATUS when order canseled
        if (request.Request.OrderStatus == OrderStatus.Cancelled)
        {
            var orderItems = await OrderItemRepository.GetAllOrderItems(request.OrderId);
            foreach (var item in orderItems.ToList())
            {
                var product = await ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ResourseNotFoundException($"Product {item.ProductId} not found");
                }
                //to do: must chek it
                product.Stock += item.Quantity;
                await ProductRepository.UpdateAsync(product);
                await OrderItemRepository.DeleteAsync(item.Id);

            }
            await ProductRepository.SaveChangesAsync();
            await OrderItemRepository.SaveChangesAsync();
            order.TotalAmount = 0;
        }

        order.OrderStatus = request.Request.OrderStatus;
        await OrderRepository.UpdateAsync(order);
        await OrderRepository.SaveChangesAsync();
        switch (request.Request.OrderStatus)
        {
            case OrderStatus.Cancelled:
                await PublishEndpoint.Publish(new OrderCancelledEvent
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    UpdateTime = DateTime.UtcNow
                });
                break;
            case OrderStatus.Completed:
                await PublishEndpoint.Publish(new OrderCompletedEvent
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.TotalAmount,
                    CompletedAt = DateTime.UtcNow
                });
                break;
            case OrderStatus.Shipped:
                await PublishEndpoint.Publish(new OrderShippedEventToCeo
                {
                    Count = 1,
                    DateTime = DateTime.UtcNow
                });
                break;
        }
        return Mapper.Map<OrderResponse>(order);
    }
}