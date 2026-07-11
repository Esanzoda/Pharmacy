using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByEmailQuery(string Email) : IRequest<CustomerResponse>;

public class GetCustomerByEmailHendler : CustomerDiBase, IRequestHandler<GetCustomerByEmailQuery, CustomerResponse>
{
    public GetCustomerByEmailHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<CustomerResponse> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetCustomerByEmailAsync(request.Email);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer with this email{request.Email} not found");
        }

        return Mapper.Map<CustomerResponse>(customer);
    }
}