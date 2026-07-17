using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver;

public class DeliverDiBase
{
    protected readonly IDeliverRepository DeliverRepository;
    protected readonly IMapper Mapper;
    protected  readonly IDistributedCache Cache;

    public DeliverDiBase(IDeliverRepository deliverRepository, IMapper mapper, IDistributedCache cache)
    {
        DeliverRepository = deliverRepository;
        Mapper = mapper;
        Cache = cache;
    }
}