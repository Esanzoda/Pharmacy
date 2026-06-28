using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICustomerService
    : IBaseService<CustomerRequest, CustomerResponse>
{
    Task<CustomerResponse?> GetCustomerByEmailAsync(string email);
    Task<CustomerResponse?> GetCustomerByPhoneAsync(string phone);
    Task<List<CustomerResponse>> GetCustomerByNameAsync(string name);
}

public class  CustomerService : BaseService<Customer, CustomerRequest, CustomerResponse>, ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerrepository, IMapper mapper)
        : base(customerrepository, mapper)
    {
        _customerRepository = customerrepository;
    }

    public async Task<CustomerResponse?> GetCustomerByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetCustomerByEmailAsync(email);
        if (customer == null)
            throw new ResourseNotFoundExeption("Customer not found");

        return Mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse?> GetCustomerByPhoneAsync(string phone)
    {
        var customer = await _customerRepository.GetCustomerByPhoneAsync(phone);
        if (customer == null)
            throw new ResourseNotFoundExeption("Customer not found");
        return Mapper.Map<CustomerResponse>(customer);
    }

    public async Task<List<CustomerResponse>> GetCustomerByNameAsync(string name)
    {
        var customer = await _customerRepository.GetCustomerByNameAsync(name);
        if (!customer.Any())
            throw new ResourseNotFoundExeption("Customer not found");
        return Mapper.Map<List<CustomerResponse>>(customer);
    }
}