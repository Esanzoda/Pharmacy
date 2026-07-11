using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByIdQuery(long Id) : IRequest<CustomerResponse>;

public class GetCustomerByIdHendler : CustomerDiBase, IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
{
    public GetCustomerByIdHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)

    {
    }
    

    public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ResourseNotFoundException("customer not found");
        }

        return Mapper.Map<CustomerResponse>(customer);
    }
}