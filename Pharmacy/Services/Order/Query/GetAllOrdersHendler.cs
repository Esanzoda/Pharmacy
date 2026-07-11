using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Query;

public record GetAllOrdersQuery(int PageNumber, int PageSize) : IRequest<List<OrderResponse>>;
public class GetAllOrdersHendler:OrderDiBase,IRequestHandler<GetAllOrdersQuery,List<OrderResponse>>
{
    public GetAllOrdersHendler(IOrderRepository orderRepository, IMapper mapper) 
        : base(orderRepository, mapper)
    {
    }

    public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await OrderRepository.GetAllByPaginationAsync(request.PageNumber, request.PageSize);
        return Mapper.Map<List<OrderResponse>>(orders);
    }
}