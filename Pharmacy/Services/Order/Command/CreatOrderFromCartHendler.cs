using AutoMapper;
using MassTransit;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;

public record CreateOrderFromCartCommand(long CustomerId, OrderType OrderType,string Address) : IRequest<OrderResponse>;

public class CreatOrderFromCartHendler : OrderDiBase, IRequestHandler<CreateOrderFromCartCommand, OrderResponse>
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

        foreach (var item in cart.CartItems)
        {
            var productCheck = await ProductRepository.GetByIdAsync(item.ProductId);
            if (productCheck == null)
            {
                throw new ResourseNotFoundException($"Product {item.ProductId} not found");
            }

            if (productCheck.Stock < item.Quantity)
            {
                throw new BusinessException($"Insufficient stock for product {productCheck.Name}: available {productCheck.Stock}, requested {item.Quantity}");
            }
        }

        string? newAddress;
        if (string.IsNullOrEmpty(request.Address))
        {
            newAddress = cart.Customer?.Address; 
        }
        newAddress=request.Address;
        
        var order = new Models.Domain.Order()
        {
            CustomerId = cart.CustomerId,
            OrderType = OrderType.Deliver,
            OrderStatus = OrderStatus.Pending,
           
            Adress = newAddress,
            //GetTime = 
            Customer = cart.Customer,
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