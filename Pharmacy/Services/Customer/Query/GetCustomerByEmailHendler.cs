using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByEmailQuery(string Email) : IRequest<CustomerResponse>;

public class GetCustomerByEmailHendler : CustomerDiBase, IRequestHandler<GetCustomerByEmailQuery, CustomerResponse>
{
    private readonly IDistributedCache _cache;
    public GetCustomerByEmailHendler(ICustomerRepository customerRepository, IMapper mapper, IDistributedCache cache)
        : base(customerRepository, mapper)
    {
        _cache = cache;
    }

    public async Task<CustomerResponse> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var key =$"CustomerByEmail-{request.Email}";
       var cachedCustomerByEmail = await _cache.GetStringAsync(key, cancellationToken);
       if (cachedCustomerByEmail is not null)
       {
         var redis=  JsonConvert.DeserializeObject<CustomerResponse>(cachedCustomerByEmail);
         if (redis is not null)
         {
             return redis;
         }
       }
        var customer = await CustomerRepository.GetCustomerByEmailAsync(request.Email);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer with this email{request.Email} not found");
        }
        await _cache.SetStringAsync(key, 
            JsonConvert.SerializeObject(customer),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow =  TimeSpan.FromDays(1)
            });

        return Mapper.Map<CustomerResponse>(customer);
    }
}