using AutoMapper;
using MediatR;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Query;

public record GetOrderByOrderStatusQuery(OrderStatus OrderStatus, int PageNumber, int PageSize)
    : IRequest<List<OrderResponse>>;
public class GetOrderByOrderStatusHendler:OrderDiBase,IRequestHandler<GetOrderByOrderStatusQuery,List<OrderResponse>>
{
    public GetOrderByOrderStatusHendler(IOrderRepository orderRepository, IMapper mapper) 
        : base(orderRepository, mapper)
    {
        
    }
    

    public async Task<List<OrderResponse>> Handle(GetOrderByOrderStatusQuery request, CancellationToken cancellationToken)
    {
       var orders =await OrderRepository.GetOrdersByOrderStatusAsync(request.OrderStatus, request.PageNumber, request.PageSize);
        return Mapper.Map<List<OrderResponse>>(orders);
    }
}