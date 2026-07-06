using AutoMapper;
using MassTransit;
using Pharmasy.Exeption;
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
}

public class OrderService : BaseService<Order, OrderRequest, OrderResponse>
    , IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPublishEndpoint _publishEndpoint;


    public OrderService(IOrderRepository orderrepository, IMapper mapper
        , IProductRepository productRepository, ICustomerRepository customerRepository,
        IPublishEndpoint publishEndpoint
        , IOrderItemRepository orderItemRepository)
        : base(orderrepository, mapper)
    {
        _orderRepository = orderrepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _publishEndpoint = publishEndpoint;
    }

    public override async Task<OrderResponse> CreateAsync(OrderRequest request)
    {
        var user = await _customerRepository.GetByIdAsync(request.CustomererId);
        if (user == null)
        {
            throw new ResourseNotFoundExeption($"Customer not found");
        }

        var order = Mapper.Map<Order>(request);

        order.OrderStatus = OrderStatus.Pending;
        //order.CreatedAt = DateTime.UtcNow;
        await _orderRepository.CreateAsync(order);

        // doesnt use method into foreach
        foreach (var item in request.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new ResourseNotFoundExeption($"Product not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new BusinessExseption($"Insufficient product stock{product.Stock}");
            }

            var exsistingOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (exsistingOrderItem != null)
            {
                exsistingOrderItem.Quantity += item.Quantity;
                exsistingOrderItem.TotalPrice = exsistingOrderItem.Quantity * product.Price;
                await _orderItemRepository.UpdateAsync(exsistingOrderItem);
                await _orderItemRepository.SavechangesAsync();
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);
                await _productRepository.SavechangesAsync();
                order.TotalAmout = order.OrderItems.Sum(x => x.TotalPrice);
            }
            else
            {
                var orderItem = Mapper.Map<OrderItem>(item);
                orderItem.Price = product.Price;
                orderItem.TotalPrice = item.Quantity * product.Price;
                await _orderItemRepository.CreateAsync(orderItem);
                await _orderItemRepository.SavechangesAsync();
                order.OrderItems.Add(orderItem);
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);
                await _productRepository.SavechangesAsync();
                order.TotalAmout = order.OrderItems.Sum(x => x.TotalPrice);
            }
        }

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SavechangesAsync();
        await _publishEndpoint.Publish(new OrderCreatedEvant()
        {
            OrderId = order.Id,
            UserId = order.CustomererId,
            TotalAmout = order.TotalAmout,
        });

        return Mapper.Map<OrderResponse>(order);
    }
//to do empty line after if


    public async Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new ResourseNotFoundExeption("OrderItem not found");
        }

        if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessExseption("Can't remove item completed or cancelled order ");
        }

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (itemToRemove == null)
        {
            throw new ResourseNotFoundExeption($"OrderItem not found");
        }

        var product = await _productRepository.GetByIdAsync(itemToRemove.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundExeption($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;
        // order.OrderItems.Remove(itemToRemove);
        // fixed to -= to =
        order.TotalAmout = order.OrderItems.Sum(x => x.TotalPrice);
        await _productRepository.UpdateAsync(product);
        await _productRepository.SavechangesAsync();
        await _orderItemRepository.DeleteAsync(itemToRemove.Id);
        await _orderRepository.UpdateAsync(order);
        await _orderItemRepository.SavechangesAsync();
        return Mapper.Map<OrderResponse>(order);
    }


    public async Task<OrderResponse> UpdateOrderStatusAsync(long orderId, UpdateOrderRequest itemrequest)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new ResourseNotFoundExeption($"Order not found");
        }

        if (order.OrderStatus == OrderStatus.Completed)
        {
            throw new BusinessExseption("Cannot update a completed order");
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
        {
            throw new BusinessExseption("Order alredy canseled");
        }

        if (order.OrderStatus == OrderStatus.Shipped)
        {
            throw new BusinessExseption("Can't update shipped order");
        }


        if (itemrequest.OrderStatus == OrderStatus.Cancelled)
        {
            await _publishEndpoint.Publish(new OrderCancelledEvant()
                {
                    OrderId = order.Id,
                    CustomererId = order.CustomererId,
                    UpdateTime = DateTime.UtcNow
                }
            );
            var orderItems = await _orderItemRepository.GetAllOrderItems(orderId);
            foreach (var item in orderItems)
            {
                //to do: must chek it
                item.Product.Stock += item.Quantity;
                order.OrderItems.Remove(item);
                await _productRepository.UpdateAsync(item.Product);
                await _productRepository.SavechangesAsync();
                await _orderItemRepository.DeleteAsync(item.Id);
                await _orderItemRepository.SavechangesAsync();
            }

            order.TotalAmout = 0;
        }

        if (itemrequest.OrderStatus == OrderStatus.Completed)
        {
            await _publishEndpoint.Publish(new OrderCompletedEvant
            {
                OrderId = order.Id,
                UserId = order.CustomererId,
                TotalAmout = order.TotalAmout
            });
        }

        order.OrderStatus = itemrequest.OrderStatus;
        await _orderRepository.UpdateAsync(order);
        await _orderItemRepository.SavechangesAsync();
        return Mapper.Map<OrderResponse>(order);
    }
}