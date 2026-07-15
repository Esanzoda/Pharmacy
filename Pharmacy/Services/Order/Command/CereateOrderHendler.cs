using AutoMapper;
using MassTransit;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Messages.Evants;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;

public record CreateOrderCommand(OrderRequest Request) : IRequest<OrderResponse>;

public class CereateOrderHendler : OrderDiBase, IRequestHandler<CreateOrderCommand, OrderResponse>
{
    public CereateOrderHendler(ICustomerRepository customerRepository, IOrderRepository orderRepository,
        IProductRepository productRepository, IOrderItemRepository orderItemRepository, IMapper mapper,
        IPublishEndpoint publishEndpoint)
        : base(customerRepository, orderRepository, productRepository, orderItemRepository, mapper, publishEndpoint)
    {
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(request.Request.CustomerId);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer not found");
        }

        var order = Mapper.Map<Models.Domain.Order>(request.Request);

        order.OrderStatus = OrderStatus.Pending;
        await OrderRepository.CreateAsync(order);
        
        foreach (var item in request.Request.OrderItems)
        {
            var product = await ProductRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new BusinessException($"Insufficient product stock{product.Stock}");
            }

            var exsistingOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (exsistingOrderItem != null)
            {
                exsistingOrderItem.Quantity += item.Quantity;
                exsistingOrderItem.TotalPrice = exsistingOrderItem.Quantity * product.Price;
            }
            else
            {
                var orderItem = Mapper.Map<OrderItem>(item);
                orderItem.OrderId = order.Id;
                orderItem.Price = product.Price;
                orderItem.TotalPrice = item.Quantity * product.Price;
                await OrderItemRepository.CreateAsync(orderItem);
                order.OrderItems.Add(orderItem);
            }

            product.Stock -= item.Quantity;
            await ProductRepository.UpdateAsync(product);
            var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
            if (request.Request.OrderType is OrderType.Deliver)
            {
                var address = request.Request.Adress;

                if (address is "1" || address is "2" || address is "3")
                {
                    totalAmount += 10;
                }
            }

            order.TotalAmount = totalAmount;
        }

        await ProductRepository.SaveChangesAsync();
        await OrderItemRepository.SaveChangesAsync();
        await OrderRepository.UpdateAsync(order);
        await OrderRepository.SaveChangesAsync();
        await PublishEndpoint.Publish(new OrderCreatedEvent()
        {
            OrderId = order.Id,
            UserId = order.CustomerId,
            TotalAmount = order.TotalAmount,
        });

        return Mapper.Map<OrderResponse>(order);
    }
}