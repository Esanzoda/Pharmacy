using AutoMapper;
using MassTransit;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;

public record CreateOrderFromCartCommand(long CustomerId): IRequest<OrderResponse>;
public class CreatOrderFromCartHendler:OrderDiBase,IRequestHandler<CreateOrderFromCartCommand,OrderResponse>
{
    public CreatOrderFromCartHendler(ICartRepository cartRepository, IMapper mapper, IOrderRepository orderRepository, 
        IOrderItemRepository orderItemRepository, IProductRepository productRepository) 
        : base(cartRepository, mapper, orderRepository, orderItemRepository, productRepository)
    {
    }

    public async Task<OrderResponse> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await CartRepository.GetCartByCustomerId(request.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException($"Cart is empty");
        }

        var order = new Models.Domain.Order()
        {
            CustomerId = cart.CustomerId,
            OrderType = OrderType.Deliver,
            OrderStatus = OrderStatus.Pending,
            Adress = cart.Customer.Address,
            //GetTime = 
            Customer =  cart.Customer,
            TotalAmount = cart.TotalAmount,
        };
        await OrderRepository.CreateAsync(order);
        await OrderRepository.SaveChangesAsync();
        foreach (var item in cart.CartItems)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                TotalPrice = item.TotalPrice
            };
            await OrderItemRepository.CreateAsync(orderItem);
            var product = await ProductRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product not found");
            }

            product.Stock -= item.Quantity;
        }

        await OrderRepository.SaveChangesAsync();
        await CartRepository.ClearCartAsync(request.CustomerId);
        return Mapper.Map<OrderResponse>(order);
    }
}