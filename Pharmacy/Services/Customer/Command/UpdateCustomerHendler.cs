using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record UpdateCustomerCommand(long Id, UpdateCustomerRequest Request) : IRequest<CustomerResponse>;

public class UpdateCustomerHendler : CustomerDiBase, IRequestHandler<UpdateCustomerCommand, CustomerResponse>
{
    private IDistributedCache _cache;

    public UpdateCustomerHendler(ICustomerRepository customerRepository, IMapper mapper, IDistributedCache cache)
        : base(customerRepository, mapper)
    {
        _cache = cache;
    }

    public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer not found with id {request.Id}");
        }

        var customerByEmail = await CustomerRepository.GetCustomerByEmailAsync(request.Request.Email);
        if (customerByEmail != null)
        {
            throw new ResourseIsAlredyExistException(
                $"Customer already exists with this email {request.Request.Email}");
        }

        var customerByPhone = await CustomerRepository.GetCustomerByPhoneAsync(request.Request.PhoneNumber);
        if (customerByPhone != null)
        {
            throw new ResourseIsAlredyExistException(
                $"Customer already exists with this phone number{request.Request.PhoneNumber}");
        }
        
        Mapper.Map(request.Request, customer);
        await CustomerRepository.UpdateAsync(customer);
        await CustomerRepository.SaveChangesAsync();

        var key = $"CustomerById-{customer.Id}";
        await _cache.RemoveAsync(key, cancellationToken);
        return Mapper.Map<CustomerResponse>(customer);
    }
}