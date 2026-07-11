using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Query;

public record GetAllCustomerByPagenationQuery(int PageNumber, int PageSize) : IRequest<List<CustomerResponse>>;

public class GetAllCustomerByPagenationHendler : CustomerDiBase,
    IRequestHandler<GetAllCustomerByPagenationQuery, List<CustomerResponse>>
{
    public GetAllCustomerByPagenationHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<List<CustomerResponse>> Handle(GetAllCustomerByPagenationQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await CustomerRepository.GetAllByPaginationAsync(request.PageNumber, request.PageSize);
        return Mapper.Map<List<CustomerResponse>>(customers);
    }
}