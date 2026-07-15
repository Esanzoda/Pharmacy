using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByIdQuery(long Id) : IRequest<CustomerResponse>;

public class GetCustomerByIdHendler : CustomerDiBase, IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
{
    private readonly IDistributedCache _cache;

    public GetCustomerByIdHendler(ICustomerRepository customerRepository, IMapper mapper, IDistributedCache cache)
        : base(customerRepository, mapper)
    {
        _cache = cache;
    }


    public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"CustomerById-{request.Id}";
        var cachedCustomer = await _cache.GetStringAsync(key, cancellationToken);
        if (cachedCustomer is not null)
        {
            var redis = JsonConvert.DeserializeObject<CustomerResponse>(cachedCustomer);

            if (redis is not null)
            {
                return redis;
            }
        }
        
        var customer = await CustomerRepository.GetByIdAsync(request.Id);
        if (customer is  null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }
        await _cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(customer), 
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            },
            cancellationToken);
        return Mapper.Map<CustomerResponse>(customer);
       
    }
}