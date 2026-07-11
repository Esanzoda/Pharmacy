using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IDelivererService : IBaseService<DeliverRequest, DeliverResponse>
{
}

public class DeliverService : BaseService<Deliver, DeliverRequest, DeliverResponse>, IDelivererService
{
    private readonly IDistributedCache _cache;

    public DeliverService(IDeliverRepository repository, IMapper mapper, IDistributedCache distributedCache)
        : base(repository, mapper, distributedCache)

    {
    }
}