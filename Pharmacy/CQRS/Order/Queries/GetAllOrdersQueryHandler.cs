using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Order.Queries;

public record GetAllOrdersQuery(
    int PageNumber,
    int PageSize) : IRequest<List<OrderResponse>>;

public class GetAllOrdersQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllOrdersQuery, List<OrderResponse>>
{
    public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<OrderResponse>>(orders);
    }
}