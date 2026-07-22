using AutoMapper;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Mappers;

public class Mappers : Profile
{
    public Mappers()
    {
        CreateMap<CartRequest, Cart>();
        CreateMap<CartItemRequest, CartItem>();
        CreateMap<CartItem, CartItemResponse>();
        CreateMap<Cart, CartResponse>()
            .ForMember(x => x.CartItemResponses,
                x => x.MapFrom(o => o.CartItems));

        CreateMap<OrderRequest, Order>()
            .ForMember(x => x.OrderItems, opt => opt.Ignore());

        CreateMap<OrderItemRequest, OrderItem>()
            .ForMember(x => x.Price, opt => opt.Ignore())
            .ForMember(x => x.TotalPrice, opt => opt.Ignore());
        CreateMap<OrderItem, OrderItemResponse>();
        CreateMap<Order, OrderResponse>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(o => o.OrderItems));

        CreateMap<PurchaseRequest, Purchase>()
            .ForMember(x => x.PurchaseItems, opt => opt.Ignore());
        CreateMap<PurchaseItemRequest, PurchaseItem>();
        CreateMap<PurchaseItem, PurchaseItemResponse>();
        CreateMap<Purchase, PurchaseResponse>()
            .ForMember(x => x.PurchaseItems, x => x.MapFrom(y => y.PurchaseItems));
        
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();
        CreateMap<UpdateCategoryRequest, Category>();
        CreateMap<Category, UpdateCategoryResponse>();
        

        CreateMap<CustomerRequest, Customer>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());
        CreateMap<Customer, CustomerResponse>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();
        

        CreateMap<EmployeeRequest, Employee>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());
        CreateMap<Employee, EmployeeResponse>();


        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();

        CreateMap<DeliverRequest, Deliver>();
        CreateMap<Deliver, DeliverResponse>();

        CreateMap<PharmacyRequest,Models.Domain.Pharmacy>();
        CreateMap<Models.Domain.Pharmacy, PharmacyResponse>();
    }
}