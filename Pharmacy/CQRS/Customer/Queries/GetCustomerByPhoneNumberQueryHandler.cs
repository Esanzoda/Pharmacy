using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Customer.Queries;

public record GetCustomerByPhoneNumberQuery(
    string PhoneNumber
) : IRequest<CustomerResponse>;

public class GetCustomerByPhoneNumberQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerByPhoneNumberQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer with this phoneNumber{request.PhoneNumber} not found");
        }

        return mapper.Map<CustomerResponse>(customer);
    }
}