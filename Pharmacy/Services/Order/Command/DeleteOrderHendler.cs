using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Order.Command;
public record DeleteOrderCommand(long OrderId):IRequest<bool>;
public class DeleteOrderHendler:OrderDiBase,IRequestHandler<DeleteOrderCommand,bool>
{
    public DeleteOrderHendler(IOrderRepository orderRepository) : base(orderRepository)
    {
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var category = await OrderRepository.DeleteAsync(request.OrderId);
        if (category is false)
        {
            throw new ResourseNotFoundException($"Order with id {request.OrderId} not found");
        }

        await OrderRepository.SaveChangesAsync();
        return category;
    }
}