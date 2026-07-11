using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Messages.Evants;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IOrderService
    : IBaseService<OrderRequest, OrderResponse>
{
    public Task<OrderResponse> UpdateOrderStatusAsync(long orderId, UpdateOrderRequest itemrequest);
    public Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId);
    public Task<OrderResponse> CreatOrderFromCartAsync(long customerId);
}

public class OrderService : BaseService<Models.Domain.Order, OrderRequest, OrderResponse>
    , IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICartRepository _cartRepository;
    private readonly IDistributedCache _cache;


    public OrderService(IOrderRepository orderrepository, IMapper mapper
        , IProductRepository productRepository, ICustomerRepository customerRepository,
        IPublishEndpoint publishEndpoint, ICartRepository cartRepository, IDistributedCache distributedCache
        , IOrderItemRepository orderItemRepository)
        : base(orderrepository, mapper, distributedCache)
    {
        _orderRepository = orderrepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _publishEndpoint = publishEndpoint;
        _cartRepository = cartRepository;
        _cache = distributedCache;
    }

    public override async Task<OrderResponse> CreateAsync(OrderRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer not found");
        }

        var order = Mapper.Map<Models.Domain.Order>(request);

        order.OrderStatus = OrderStatus.Pending;
        await _orderRepository.CreateAsync(order);

        // doesnt use method into foreach
        foreach (var item in request.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
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
                await _orderItemRepository.CreateAsync(orderItem);
                order.OrderItems.Add(orderItem);
            }

            product.Stock -= item.Quantity;
            await _productRepository.UpdateAsync(product);
            order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        }

        await _productRepository.SaveChangesAsync();
        await _orderItemRepository.SaveChangesAsync();
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
        await _publishEndpoint.Publish(new OrderCreatedEvent()
        {
            OrderId = order.Id,
            UserId = order.CustomerId,
            TotalAmount = order.TotalAmount,
        });

        return Mapper.Map<OrderResponse>(order);
    }
//to do empty line after if


    public async Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new ResourseNotFoundException("OrderItem not found");
        }

        if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessException("Can't remove item completed or cancelled order ");
        }

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (itemToRemove == null)
        {
            throw new ResourseNotFoundException($"OrderItem not found");
        }

        var product = await _productRepository.GetByIdAsync(itemToRemove.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundException($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();

        order.OrderItems.Remove(itemToRemove);
        order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();

        await _orderItemRepository.DeleteAsync(itemToRemove.Id);
        await _orderItemRepository.SaveChangesAsync();

        return Mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> CreatOrderFromCartAsync(long customerId)
    {
        var cart = await _cartRepository.GetCartByCustomerId(customerId);
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
        await _orderRepository.CreateAsync(order);
        await _orderRepository.SaveChangesAsync();
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
            await _orderItemRepository.CreateAsync(orderItem);
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new ResourseNotFoundException($"Product not found");
            }

            product.Stock -= item.Quantity;
        }

        await _orderRepository.SaveChangesAsync();
        await _cartRepository.ClearCartAsync(customerId);
        return Mapper.Map<OrderResponse>(order);
    }


    public async Task<OrderResponse> UpdateOrderStatusAsync(long orderId, UpdateOrderRequest itemrequest)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
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


        if (itemrequest.OrderStatus == OrderStatus.Cancelled)
        {
            var orderItems = await _orderItemRepository.GetAllOrderItems(orderId);
            foreach (var item in orderItems)
            {
                //to do: must chek it
                item.Product.Stock += item.Quantity;
                order.OrderItems.Remove(item);
                await _productRepository.UpdateAsync(item.Product);
                await _productRepository.SaveChangesAsync();
                await _orderItemRepository.DeleteAsync(item.Id);
                await _orderItemRepository.SaveChangesAsync();
            }

            order.TotalAmount = 0;
        }

        order.OrderStatus = itemrequest.OrderStatus;
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
        if (itemrequest.OrderStatus == OrderStatus.Cancelled)
        {
            await _publishEndpoint.Publish(new OrderCancelledEvent()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    UpdateTime = DateTime.UtcNow
                }
            );
        }

        if (itemrequest.OrderStatus == OrderStatus.Completed)
        {
            await _publishEndpoint.Publish(new OrderCompletedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount
            });
        }

        order.OrderStatus = itemrequest.OrderStatus;
        await _orderRepository.UpdateAsync(order);
        await _orderItemRepository.SaveChangesAsync();
        return Mapper.Map<OrderResponse>(order);
    }
}