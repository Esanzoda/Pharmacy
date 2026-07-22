using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.CQRS.Auth.Commands;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Commands;

public record UpdateCustomerPasswordCommand(
    long Id,
    string NewPassword) : IRequest<CustomerResponse>;

public class UpdateCustomerPasswordHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext,
    IMapper mapper,
    IMediator mediator) : IRequestHandler<UpdateCustomerPasswordCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(UpdateCustomerPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FindAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

        customer.PasswordHash = await mediator.Send(new PasswordHashCommand(request.NewPassword), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CustomerById-{customer.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return mapper.Map<CustomerResponse>(customer);
    }
}