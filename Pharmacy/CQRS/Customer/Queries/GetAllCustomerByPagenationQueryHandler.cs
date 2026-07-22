using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetAllCustomerByPagenationQuery(
    int PageNumber,
    int PageSize
) : IRequest<List<CustomerResponse>>;

public class GetAllCustomerByPagenationHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCustomerByPagenationQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetAllCustomerByPagenationQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await dbContext.Customers
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<CustomerResponse>>(customers);
    }
}