using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Command;
public record DeleteDeliverCommand(int Id) : IRequest<bool>;
public class DeleteDeliverHendler:DeliverDiBase,IRequestHandler<DeleteDeliverCommand,bool>
{
    public DeleteDeliverHendler(IDeliverRepository deliverRepository, IMapper mapper, IDistributedCache cache) 
        : base(deliverRepository, mapper, cache)
    {
    }

    public async Task<bool> Handle(DeleteDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await DeliverRepository.DeleteAsync(request.Id);
        if (deliver is false)
        {
            throw new ResourseNotFoundException("Deliver not found");
        }
        var key = $"DeliverById-{request.Id}";
        await Cache.RemoveAsync(key, cancellationToken);

        return true;
    }
}