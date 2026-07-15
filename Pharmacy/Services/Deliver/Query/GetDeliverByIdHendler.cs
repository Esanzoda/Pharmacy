using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Query;

public record GetDeliverByIdQuery(long Id) : IRequest<DeliverResponse>;
public class GetDeliverByIdHendler:IRequestHandler<GetDeliverByIdQuery,DeliverResponse>
{
    private readonly IMapper _mapper;
    private readonly IDeliverRepository _deliverRepository;
    private readonly IDistributedCache _cache;

    public GetDeliverByIdHendler(IMapper mapper, IDeliverRepository deliverRepository, IDistributedCache cache)
    {
        _mapper = mapper;
        _deliverRepository = deliverRepository;
        _cache = cache;
    }

    public async Task<DeliverResponse> Handle(GetDeliverByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"DeliverById-{request.Id}";
      var cached=  await _cache.GetStringAsync(key, cancellationToken);
      if (cached is not null)
      {
          return _mapper.Map<DeliverResponse>(cached);
      }
      var deliver =await _deliverRepository.GetByIdAsync(request.Id);
      if (deliver is null)
      {
          throw new ResourseNotFoundException("Deliver not found");
      }
      await _cache.SetStringAsync(key,
          JsonConvert.SerializeObject(deliver),
          new DistributedCacheEntryOptions()
          {
              AbsoluteExpirationRelativeToNow =  TimeSpan.FromDays(1)
          });
      return _mapper.Map<DeliverResponse>(deliver);
          
    }
}