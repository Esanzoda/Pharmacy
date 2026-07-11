using AutoMapper;
using MassTransit;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order;

public class OrderDiBase
{
    protected readonly ICustomerRepository CustomerRepository;
    protected readonly IOrderRepository OrderRepository;
    protected readonly IProductRepository ProductRepository;
    protected readonly IOrderItemRepository OrderItemRepository;
    protected readonly IMapper Mapper;
    protected readonly IPublishEndpoint PublishEndpoint;
    protected readonly ICartRepository CartRepository;

    public OrderDiBase(IOrderRepository orderRepository)
    {
        OrderRepository = orderRepository;
    }

    public OrderDiBase(IOrderRepository orderRepository, IMapper mapper)
    {
        OrderRepository = orderRepository;
        Mapper = mapper;
    }

    public OrderDiBase(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository
        , IProductRepository productRepository)
    {
        ProductRepository = productRepository;
        OrderItemRepository = orderItemRepository;
        OrderRepository = orderRepository;
    }

    public OrderDiBase(ICartRepository cartRepository, IMapper mapper, IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository, IProductRepository productRepository)
    {
        CartRepository = cartRepository;
        Mapper = mapper;
        OrderRepository = orderRepository;
        OrderItemRepository = orderItemRepository;
        ProductRepository = productRepository;
    }

    public OrderDiBase(ICustomerRepository customerRepository, IOrderRepository orderRepository,
        ProductRepository productRepository, IOrderItemRepository orderItemRepository, IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        CustomerRepository = customerRepository;
        OrderRepository = orderRepository;
        ProductRepository = productRepository;
        OrderItemRepository = orderItemRepository;
        Mapper = mapper;
        PublishEndpoint = publishEndpoint;
    }
}