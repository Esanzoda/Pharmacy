using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer;

public class CustomerDiBase
{
    protected readonly ICustomerRepository CustomerRepository;
    protected readonly IMapper Mapper;

    public CustomerDiBase(ICustomerRepository customerRepository, IMapper mapper)
    {
        CustomerRepository = customerRepository;
        Mapper = mapper;
        
    }
}