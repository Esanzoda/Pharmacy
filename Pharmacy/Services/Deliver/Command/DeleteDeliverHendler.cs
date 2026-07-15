using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Command;
public record DeleteDeliverCommand(int Id) : IRequest<bool>;
public class DeleteDeliverHendler:IRequestHandler<DeleteDeliverCommand,bool>
{
    private readonly IDeliverRepository _deliverRepository;
  private  readonly IDistributedCache _cache;

    public DeleteDeliverHendler(IDeliverRepository deliverRepository, IDistributedCache cache)
    {
        _deliverRepository = deliverRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await _deliverRepository.DeleteAsync(request.Id);
        if (deliver is false)
        {
            throw new ResourseNotFoundException("Deliver not found");
        }
        var key = $"DeliverById-{request.Id}";
        await _cache.RemoveAsync(key, cancellationToken);

        return true;
    }
}