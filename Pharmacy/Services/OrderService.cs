using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IOrderService
    : IBaseService< OrderRequest, OrderResponse>
{
    public Task<OrderResponse> CreateOnlineOrderAsync(OrderOnlineRequest request);
    public Task<OrderResponse> UpdateOrderAsync(long orderId, UpdateOrder itemrequest);
    public Task<OrderResponse> AddItemToOrderAsync(long orderId, OrderItemRequest itemrequest);
    public  Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId);
}
public class OrderService:BaseService<Order,OrderRequest,OrderResponse>
    ,IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICustomerRepository _customerRepository;
    public OrderService(IOrderRepository orderrepository, IMapper mapper
        ,IProductRepository productRepository,ICustomerRepository customerRepository,IEmployeeRepository employeeRepository
        ,IOrderItemRepository orderItemRepository)
        : base(orderrepository, mapper)
    {
        _orderRepository = orderrepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _customerRepository=customerRepository;
        _employeeRepository = employeeRepository;
    }

    public  async Task<OrderResponse> CreateOnlineOrderAsync(OrderOnlineRequest request)
    {
        var customer=await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new ResourseNotFoundExeption($"Customer not found");  
        
        var order = _mapper.Map<Order>(request);
        order.CreateAt = DateTime.Now;
        order.Totalprise = 0;
        await _orderRepository.CreateAsync(order);
        foreach (var item in request.OrderItems)
        {
            await AddItemToOrderAsync(order.Id, item);
        }
        return _mapper.Map<OrderResponse>(order);
    }
    public override async Task<OrderResponse> CreateAsync(OrderRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
            throw new ResourseNotFoundExeption($"Employee not found");
        var order = _mapper.Map<Order>(request);
        order.CreateAt = DateTime.Now;
        order.Totalprise = 0;
        await _orderRepository.CreateAsync(order);
        return _mapper.Map<OrderResponse>(order);
    }


    public async Task<OrderResponse> AddItemToOrderAsync(long orderId, OrderItemRequest itemrequest)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) 
            throw new Exception("Заказ не найден");
        var product = await _productRepository.GetByIdAsync(itemrequest.ProductId);
            if(product == null)
                throw new ResourseNotFoundExeption($"Product with id {itemrequest.ProductId} not found");
            
            if(product.Stock< itemrequest.Quantity)
                throw new BusinessExseption($"Product stock({product.Stock}) Request: {itemrequest.ProductId}");
            
            var existingOrderItem = await _orderItemRepository.GetByIdAsync(itemrequest.ProductId);
            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity += itemrequest.Quantity;
                existingOrderItem.TotalPrice=existingOrderItem.Quantity*product.OrderPrice;
            }
            
            var orderItem = _mapper.Map<OrderItem>(itemrequest);
            orderItem.CreateAt = DateTime.Now;
            orderItem.TotalPrice = existingOrderItem.Quantity*product.OrderPrice;
            order.OrderItems.Add(orderItem);
            
            product.Stock -= itemrequest.Quantity;
            await _productRepository.UpdateAsync(product);
            order.Totalprise += order.OrderItems.Sum(x => x.TotalPrice);
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponse>(order);
    }
    public async Task<OrderResponse> RemoveItemFromOrderAsync(long orderId, long orderItemId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) 
            throw new ResourseNotFoundExeption("Order not found");
        
        if(order.OrderStatus == OrderStatus.Completed||order.OrderStatus == OrderStatus.Cancelled)
        throw new BusinessExseption("Order is canceled or order is canceled");

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (itemToRemove == null)
        {
            throw new ResourseNotFoundExeption($"OrderItem with id {orderItemId} does not exist");
          
        }
            var product = await _productRepository.GetByIdAsync(itemToRemove.ProductId);
            if (product != null)
            {
                product.Stock += itemToRemove.Quantity;
                await _orderRepository.UpdateAsync(order);
            }

            order.OrderItems.Remove(itemToRemove);
            order.Totalprise -= order.OrderItems.Sum(x => x.TotalPrice);
            await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderResponse>(order);
    }
    
    public async Task<OrderResponse> UpdateOrderAsync(long orderId, UpdateOrder itemrequest)
    {
        var order =await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ResourseNotFoundExeption($"Order not found");
        
        if (order.OrderStatus == OrderStatus.Completed)
            throw new BusinessExseption("You cannot update complete order");
        if (order.OrderStatus == OrderStatus.Cancelled)
            throw new BusinessExseption("Order alredy canseled");
        if (order.OrderStatus == OrderStatus.Shipped)
            throw new BusinessExseption("You cannot update shipped order");
        if (itemrequest.OrderStatus == OrderStatus.Cancelled)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }
            order.Totalprise = 0;
        }
        order.OrderStatus=itemrequest.OrderStatus;
        order.UpdateAt = DateTime.Now;
        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<OrderResponse>(order);
    }
}