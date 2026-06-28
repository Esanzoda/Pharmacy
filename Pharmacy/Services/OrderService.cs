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
    public Task<OrderResponse> UpdateOrderAsync(long orderId, UpdateOrder itemrequest);
    public Task<OrderResponse> AddItemToOrderAsync(long orderId, OrderItemRequest itemrequest);
    public Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId);
    public Task<OrderResponse> CreateReservationOrderAsync(OrderReservationRequest reservationRequest);
}

public class OrderService : BaseService<Order, OrderRequest, OrderResponse>
    , IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository orderrepository, IMapper mapper
        , IProductRepository productRepository, ICustomerRepository customerRepository,
        IPublishEndpoint publishEndpoint, IUserRepository userRepository
        , IOrderItemRepository orderItemRepository)
        : base(orderrepository, mapper)
    {
        _orderRepository = orderrepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _publishEndpoint = publishEndpoint;
        _userRepository = userRepository;
    }

    public override async Task<OrderResponse> CreateAsync(OrderRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new ResourseNotFoundExeption($"Customer not found");

        var order = Mapper.Map<Order>(request);
        order.CreateAt = DateTime.UtcNow;
        order.OrderStatus = OrderStatus.Pending;
        await _orderRepository.CreateAsync(order);
        // doesnt use method into foreach
        /*  foreach (var item in request.OrderItems)
          {
              await AddItemToOrderAsync(order.Id, item);
          }*/
        await _publishEndpoint.Publish(new OrderCreatedEvant()
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmout = order.TotalAmout,
        });

        return Mapper.Map<OrderResponse>(order);
    }


    public async Task<OrderResponse> AddItemToOrderAsync(long orderId, OrderItemRequest itemrequest)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ResourseNotFoundExeption($"Ordernot not found");
        var product = await _productRepository.GetByIdAsync(itemrequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption($"Product not found");

        if (product.Stock < itemrequest.Quantity)
            throw new BusinessExseption(
                $"Insufficient product stock{product.Stock} for the requested quantity {itemrequest.Quantity}");

        var existingOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == itemrequest.ProductId);
        if (existingOrderItem != null)
        {
            existingOrderItem.Quantity += itemrequest.Quantity;
            existingOrderItem.TotalPrice = existingOrderItem.Quantity * product.Price;
            await _orderItemRepository.UpdateAsync(existingOrderItem);
        }
        else
        {
            var orderItem = Mapper.Map<OrderItem>(itemrequest);
            orderItem.CreateAt = DateTime.UtcNow;
            orderItem.Price = product.Price;
            orderItem.TotalPrice = itemrequest.Quantity * product.Price;
            order.OrderItems.Add(orderItem);
            product.Stock -= itemrequest.Quantity;
            order.TotalAmout += order.OrderItems.Sum(x => x.TotalPrice);
            await _productRepository.UpdateAsync(product);
            await _orderItemRepository.UpdateAsync(orderItem);
        }
            order.TotalAmout += order.OrderItems.Sum(x => x.TotalPrice);
        await _orderRepository.UpdateAsync(order);

        return Mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ResourseNotFoundExeption("OrderItem not found");

        if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
            throw new BusinessExseption("Can't remove item completed or cancelled order ");

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (itemToRemove == null)
            throw new ResourseNotFoundExeption($"OrderItem not found");

        var product = await _productRepository.GetByIdAsync(itemToRemove.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption($"Product not found");

        product.Stock += itemToRemove.Quantity;

        order.OrderItems.Remove(itemToRemove);
        product.Stock += itemToRemove.Quantity;
        order.TotalAmout -= order.OrderItems.Sum(x => x.TotalPrice);
        await _productRepository.UpdateAsync(product);
        await _orderRepository.UpdateAsync(order);
        return Mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> CreateReservationOrderAsync(OrderReservationRequest reservationRequest)
    {
        var customer = await _customerRepository.GetByIdAsync(reservationRequest.CustomerId);
        if (customer == null)
            throw new ResourseNotFoundExeption($"Customer not found");
        var order = Mapper.Map<Order>(reservationRequest);
        order.CreateAt = DateTime.UtcNow;
        order.TotalAmout = 0;
        await _orderRepository.CreateAsync(order);
        foreach (var item in reservationRequest.OrderItemRequests)
        {
            await AddItemToOrderAsync(order.Id, item);
        }


        return Mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> UpdateOrderAsync(long orderId, UpdateOrder itemrequest)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ResourseNotFoundExeption($"Order not found");

        if (order.OrderStatus == OrderStatus.Completed)
            throw new BusinessExseption("Cannot update a cancelled order");

        if (itemrequest.OrderStatus == OrderStatus.Completed)
        {
            await _publishEndpoint.Publish(new OrderCompletedEvant
            {
                OrderId = order.Id,
                UserId = order.UserId,
                TotalAmout = order.TotalAmout
            });
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
            throw new BusinessExseption("Order alredy canseled");

        if (order.OrderStatus == OrderStatus.Shipped)
            throw new BusinessExseption("Can't update shipped order");
       

        if (itemrequest.OrderStatus == OrderStatus.Cancelled)
        {
            var orderItems = await _orderItemRepository.GetAllOrderItems(orderId);
            foreach (var item in orderItems)
            {
                item.Product.Stock += item.Quantity;
                order.OrderItems.Remove(item);
            }

            order.TotalAmout = 0;
        }

        order.OrderStatus = itemrequest.OrderStatus;
        order.UpdateAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order);
        return Mapper.Map<OrderResponse>(order);
    }
}