using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Interfaces;

namespace Pharmasy.CQRS.Deliver.Commands;

public record DeleteDeliverCommand(
    int Id
) : IRequest<bool>;

public class DeleteDeliverHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache
) : IRequestHandler<DeleteDeliverCommand, bool>
{
    public async Task<bool> Handle(DeleteDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync(request.Id, cancellationToken);
        if (deliver is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

        dbContext.Delivers
            .Remove(deliver);
        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"DeliverById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return true;
    }
}