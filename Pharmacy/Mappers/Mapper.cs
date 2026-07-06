using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Purchase = Pharmasy.Models.Domain.Purchase;

namespace Pharmasy.AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<CartRequest, Cart>();
        CreateMap<CartItemRequest, CartItem>();
        CreateMap<CartItem, CartItemResponse>();
        CreateMap<Cart, CartResponse>()
            .ForMember(x => x.CartItemResponses,
                x => x.MapFrom(x => x.CartItems));

        CreateMap<OrderRequest, Order>();
        CreateMap<OrderItemRequest, OrderItem>()
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
        CreateMap<OrderItem, OrderItemResponse>();
        CreateMap<Order, OrderResponse>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(x => x.OrderItems));

        CreateMap<PurchaseRequest, Purchase>();
        CreateMap<PurchaseItemRequest, PurchaseItem>();
        CreateMap<PurchaseItem, PurchaseItemResponse>();
        CreateMap<Purchase, PurchaseResponse>()
            .ForMember(x => x.PurchaseItems,
                x => x.MapFrom(y => y.PurchaseItems));

        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();

        CreateMap<CustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();

        CreateMap<EmployeRequest, Employee>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.Ignore());
        CreateMap<Employee, EmployeResponse>();


        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();

        CreateMap<DeliverRequest, Deliver>();
        CreateMap<Deliver, DeliverResponse>();
    }
}