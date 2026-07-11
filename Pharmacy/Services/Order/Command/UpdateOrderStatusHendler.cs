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

public class UpdateOrderStatusHendler : OrderDiBase,IRequestHandler<UpdateOrderStatusCommand, OrderResponse>
{
    public UpdateOrderStatusHendler(ICustomerRepository customerRepository, IOrderRepository orderRepository, ProductRepository productRepository, IOrderItemRepository orderItemRepository, IMapper mapper, IPublishEndpoint publishEndpoint) : base(customerRepository, orderRepository, productRepository, orderItemRepository, mapper, publishEndpoint)
    {
    }

    public async Task<OrderResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new ResourseNotFoundException($"Order not found");
        }

        if (order.OrderStatus == OrderStatus.Completed)
        {
            throw new BusinessException("Cannot update a completed order");
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessException("Order alredy canseled");
        }

        if (order.OrderStatus == OrderStatus.Shipped)
        {
            throw new BusinessException("Can't update shipped order");
        }


        if (request.Request.OrderStatus == OrderStatus.Cancelled)
        {
            var orderItems = await OrderItemRepository.GetAllOrderItems(request.OrderId);
            foreach (var item in orderItems)
            {
                //to do: must chek it
                item.Product.Stock += item.Quantity;
                order.OrderItems.Remove(item);
                await ProductRepository.UpdateAsync(item.Product);
                await ProductRepository.SaveChangesAsync();
                await OrderItemRepository.DeleteAsync(item.Id);
                await OrderItemRepository.SaveChangesAsync();
            }

            order.TotalAmount = 0;
        }

        order.OrderStatus = request.Request.OrderStatus;
        await OrderRepository.UpdateAsync(order);
        await OrderRepository.SaveChangesAsync();
        if (request.Request.OrderStatus == OrderStatus.Cancelled)
        {
            await PublishEndpoint.Publish(new OrderCancelledEvent()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    UpdateTime = DateTime.UtcNow
                }
            );
        }

        if (request.Request.OrderStatus == OrderStatus.Completed)
        {
            await PublishEndpoint.Publish(new OrderCompletedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount
            });
        }

        order.OrderStatus = request.Request.OrderStatus;
        await OrderRepository.UpdateAsync(order);
        await OrderItemRepository.SaveChangesAsync();
        return Mapper.Map<OrderResponse>(order);
    }
}