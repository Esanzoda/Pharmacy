using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetCustomerByPhoneNumberQuery(string PhoneNumber) : IRequest<CustomerResponse>;

public class GetCustomerByPhoneNumberHendler : CustomerDiBase,
    IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponse>
{
    public GetCustomerByPhoneNumberHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<CustomerResponse> Handle(GetCustomerByPhoneNumberQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetCustomerByPhoneAsync(request.PhoneNumber);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer with this phoneNumber{request.PhoneNumber} not found");
        }

        return Mapper.Map<CustomerResponse>(customer);
    }
}