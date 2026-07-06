using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

//to do chek  
namespace Pharmasy.Services;

public interface ICustomerService
    : IBaseService<CustomerRequest, CustomerResponse>
{
    Task<CustomerResponse> GetCustomerByEmailAsync(string email);
    Task<CustomerResponse> GetCustomerByPhoneAsync(string phone);
    Task<List<CustomerResponse>> GetCustomerByNameAsync(string name);
}

public class CustomerService : BaseService<Customer, CustomerRequest, CustomerResponse>, ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerrepository, IMapper mapper)
        : base(customerrepository, mapper)
    {
        _customerRepository = customerrepository;
    }

    public async override Task<CustomerResponse> CreateAsync(CustomerRequest request)
    {
        var customerByEmail = await _customerRepository.GetCustomerByEmailAsync(request.Email);
        if (customerByEmail != null)
        {
        }

        var customerByPhone = await _customerRepository.GetCustomerByPhoneAsync(request.Phonenumber);
        if (customerByPhone != null)
        {
            throw new ResourseIsAlredyExsistExeption(
                $"Customer already exists with this phone number{request.Phonenumber}");
        }

        var newCustomer = Mapper.Map<Customer>(request);
        await _customerRepository.CreateAsync(newCustomer);
        await _customerRepository.SavechangesAsync();
        return Mapper.Map<CustomerResponse>(newCustomer);
    }

    public async override Task<CustomerResponse> UpdateAsync(long id, CustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new ResourseNotFoundExeption($"Customer not found with id {id}");
        }

        var customerByEmail = await _customerRepository.GetCustomerByEmailAsync(request.Email);
        if (customerByEmail != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Customer already exists with this email {request.Email}");
        }

        var customerByPhone = await _customerRepository.GetCustomerByPhoneAsync(request.Phonenumber);
        if (customerByPhone != null)
        {
            throw new ResourseIsAlredyExsistExeption(
                $"Customer already exists with this phone number{request.Phonenumber}");
        }

        Mapper.Map(request, customer);
        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SavechangesAsync();
        return Mapper.Map<CustomerResponse>(customerByEmail);
    }

    public async Task<CustomerResponse> GetCustomerByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetCustomerByEmailAsync(email);
        if (customer == null)
        {
            throw new ResourseNotFoundExeption($"Customer with this email{email} not found");
        }

        return Mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse> GetCustomerByPhoneAsync(string phone)
    {
        var customer = await _customerRepository.GetCustomerByPhoneAsync(phone);
        if (customer == null)
        {
            throw new ResourseNotFoundExeption($"Customer with this number{phone} not found");
        }

        return Mapper.Map<CustomerResponse>(customer);
    }

    public async Task<List<CustomerResponse>> GetCustomerByNameAsync(string name)
    {
        var customer = await _customerRepository.GetCustomerByNameAsync(name);
        if (!customer.Any())
        {
            throw new ResourseNotFoundExeption($"Customer with this name{name} not found");
        }

        return Mapper.Map<List<CustomerResponse>>(customer);
    }
}