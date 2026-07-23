using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Commands;

public record DeleteCustomerCommand(
    long Id
) : IRequest<bool>;

public class DeleteCustomerHandler(
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException("Customer not found");
        }

        dbContext.Customers
            .Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"CustomerById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return true;
    }
}