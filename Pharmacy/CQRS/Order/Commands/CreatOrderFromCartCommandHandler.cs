using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.CQRS.Cart.Commands;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Messages.Events;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Order.Commands;

public record CreateOrderFromCartCommand(
    long CustomerId,
    OrderType OrderType,
    string Address) : IRequest<OrderResponse>;

public class CreatOrderFromCartHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IMediator mediator,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateOrderFromCartCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x=>x.Customer)
            .Include(x=>x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        if (cart == null && cart!.CartItems.Any())
        {
            throw new ResourseNotFoundException($"Cart is empty");
        }


        string? newAddress;
        if (string.IsNullOrEmpty(request.Address))
        {
            newAddress = cart.Customer?.Address;
        }
        else
        {
            newAddress = request.Address;
        }


        var order = new Models.Domain.Order()
        {
            CustomerId = cart.CustomerId,
            OrderType = request.OrderType,
            OrderStatus = OrderStatus.Pending,
            Adress = newAddress,
            Customer = cart.Customer,
            TotalAmount = cart.TotalAmount,
        };
        await dbContext.Orders
            .AddAsync(order, cancellationToken);
        foreach (var item in cart.CartItems)
        {
            if (item != null)
            {
                var product = await dbContext.Products
                    .FindAsync(item.ProductId, cancellationToken);
                if (product == null)
                {
                    throw new ResourseNotFoundException($"Product {item.ProductId} not found");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new BusinessException(
                        $"Insufficient stock for product {product.Name}: available {product.Stock}, requested {item.Quantity}");
                }

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price,
                    TotalPrice = product.Price*item.Quantity
                };
                order.OrderItems.Add(orderItem);
                await dbContext.OrderItems
                    .AddAsync(orderItem, cancellationToken);
                product.Stock -= item.Quantity;
            }
        }

        var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        decimal delivePrice = 0;
        if (request.OrderType is OrderType.Deliver)
        {
            var address = newAddress;

            if (address is "1" || address is "2" || address is "3")
            {
                delivePrice = 10;
            }

            order.TotalAmount = delivePrice + totalAmount;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await mediator.Send(new ClearCartCommand(request.CustomerId), cancellationToken);
        await publishEndpoint.Publish(new OrderCreatedEvent()
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
        }, cancellationToken);
        return mapper.Map<OrderResponse>(order);
    }
}