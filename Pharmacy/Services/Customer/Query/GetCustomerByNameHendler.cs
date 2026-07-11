using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByNameQuery(string Name) : IRequest<List<CustomerResponse>>;

public class GetCustomerByNameHendler : CustomerDiBase, IRequestHandler<GetCustomerByNameQuery, List<CustomerResponse>>
{
    public GetCustomerByNameHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<List<CustomerResponse>> Handle(GetCustomerByNameQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetCustomerByNameAsync(request.Name);
        if (!customer.Any())
        {
            throw new ResourseNotFoundException($"Customer with this name{request.Name} not found");
        }

        return Mapper.Map<List<CustomerResponse>>(customer);
    }
}