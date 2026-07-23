using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Commands;

public record DeleteOrderCommand(
    long OrderId) : IRequest<bool>;

public class DeleteOrderHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteOrderCommand, bool>
{
    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .FindAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            throw new RecourseNotFoundException($"Order with id {request.OrderId} not found");
        }

        dbContext.Orders
            .Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}