using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Query;
public record GetOrderByIdQuery(long Id):IRequest<OrderResponse>;
public class GetOrderByIdHendler:OrderDiBase,IRequestHandler<GetOrderByIdQuery,OrderResponse>
{
    public GetOrderByIdHendler(IOrderRepository orderRepository, IMapper mapper)
        : base(orderRepository, mapper)
    {
    }

    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await OrderRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ResourseNotFoundException("Order not found");
        }

        return Mapper.Map<OrderResponse>(customer);
    }
}