using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Deliver.Queries;

public record GetDeliverByIdQuery(
    long Id
) : IRequest<DeliverResponse>;

public class GetDeliverByIdHandler(
    IMapper mapper,
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<GetDeliverByIdQuery, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(GetDeliverByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"DeliverById-{request.Id}";
        var cached = await cache.GetStringAsync(key, cancellationToken);
        if (cached is not null)
        {
            return mapper.Map<DeliverResponse>(cached);
        }

        var deliver = await dbContext.Delivers
            .FindAsync(request.Id, cancellationToken);
        if (deliver is null)
        {
            throw new ResourseNotFoundException("Deliver not found");
        }

        var response = mapper.Map<DeliverResponse>(deliver);
        await cache.SetStringAsync(key,
            JsonConvert.SerializeObject(response),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            }, cancellationToken);
        return response;
    }
}