using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetDeliverByIdQuery(
    long Id) : IRequest<DeliverResponse>;

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
            throw new RecourseNotFoundException("Deliver not found");
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