using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByNameQuery(
    string Name
) : IRequest<List<CustomerResponse>>;

public class GetCustomerByNameQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<GetCustomerByNameQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetCustomerByNameQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        if (customer is null)
        {
            throw new ResourseNotFoundException($"Customer with this name{request.Name} not found");
        }

        return mapper.Map<List<CustomerResponse>>(customer);
    }
}