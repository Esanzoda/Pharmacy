using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Customer.Queries;

public record GetCustomerByEmailQuery(
    string Email
) : IRequest<CustomerResponse>;

public class GetCustomerByEmailHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IDistributedCache cache)
    : IRequestHandler<GetCustomerByEmailQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var key = $"CustomerByEmail-{request.Email}";
        var cachedCustomerByEmail = await cache.GetStringAsync(key, cancellationToken);
        if (cachedCustomerByEmail is not null)
        {
            var redis = JsonConvert.DeserializeObject<CustomerResponse>(cachedCustomerByEmail);
            if (redis is not null)
            {
                return redis;
            }
        }

        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer with this email{request.Email} not found");
        }

        var response = mapper.Map<CustomerResponse>(customer);
        await cache.SetStringAsync(key,
            JsonConvert.SerializeObject(response),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            }, token: cancellationToken);

        return response;
    }
}