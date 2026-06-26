using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Purchase = Pharmasy.Models.Domain.Purchase;

namespace Pharmasy.AutoMapper;

public class Mapper:Profile
{
    public Mapper()
    {
        CreateMap<CartRequest, Cart>();
        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.CartItemResponses, opt => 
                opt.MapFrom(src => src.CartItems));
        
        CreateMap<CartItemRequest, CartItem>();
        CreateMap<CartItem, CartItemResponse>();
        
        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();
        
        CreateMap<CustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();
        
        CreateMap<EmployeRequest, Employee>();
        CreateMap<Employee, EmployeResponse>();
        
        CreateMap<OrderDeliverRequest, Order>();
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.OrderItemResponses, opt =>
                opt.MapFrom(src => src.OrderItems));
        
        CreateMap<OrderReservationRequest, Order>();
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.OrderItemResponses, opt =>
                opt.MapFrom(src => src.OrderItems));

        
        
        
        CreateMap<OrderItemRequest, OrderItem>();
        CreateMap<OrderItem, OrderItemResponse>();
        
        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
        
        CreateMap<PurchaseRequest, Purchase>();
        CreateMap<Purchase, PurchaseResponse>();
        
        CreateMap<PurchaseItemRequest, PurchaseItem>();
        CreateMap<PurchaseItem, PurchaseItemResponse>();
        
        CreateMap<SupplierRequest, Supplier>();
        CreateMap<Supplier, SupplierResponse>();
    }
    
}