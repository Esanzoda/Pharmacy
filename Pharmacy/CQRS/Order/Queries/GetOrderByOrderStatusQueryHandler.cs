using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Order.Queries;

public record GetOrderByOrderStatusQuery(
    OrderStatus OrderStatus,
    int PageNumber,
    int PageSize)
    : IRequest<List<OrderResponse>>;

public class GetOrderByOrderStatusQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByOrderStatusQuery, List<OrderResponse>>
{
    public async Task<List<OrderResponse>> Handle(GetOrderByOrderStatusQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.OrderStatus == request.OrderStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);


        return mapper.Map<List<OrderResponse>>(orders);
    }
}