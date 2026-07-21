using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Order.Queries;

public record GetOrderByIdQuery(
    long Id) : IRequest<OrderResponse>;

public class GetOrderByIdQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer == null)
        {
            throw new ResourseNotFoundException("Order not found");
        }

        return mapper.Map<OrderResponse>(customer);
    }
}