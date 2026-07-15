using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record UpdateCustomerPasswordCommand(long Id, string NewPassword) : IRequest<CustomerResponse>;

public class UpdateCustomerPasswordHendler : CustomerDiBase, IRequestHandler<UpdateCustomerPasswordCommand, CustomerResponse>
{
    private readonly IDistributedCache _cache;

    public UpdateCustomerPasswordHendler(ICustomerRepository customerRepository, IMapper mapper, IDistributedCache cache) :
        base(customerRepository, mapper)
    {
        _cache = cache;
    }

    public async Task<CustomerResponse> Handle(UpdateCustomerPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

      // customer.Password = request.NewPassword;
        await CustomerRepository.SaveChangesAsync();

        var key = $"CustomerById-{customer.Id}";
        await _cache.RemoveAsync(key, cancellationToken);

        return Mapper.Map<CustomerResponse>(customer);
    }
}