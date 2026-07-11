using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetEmployeeByIdQuery(long Id) : IRequest<EmployeeResponse>;
public class GetEmployeeByIdHendler:EmployeDiBase,IRequestHandler<GetEmployeeByIdQuery,EmployeeResponse>
{
    public GetEmployeeByIdHendler(IEmployeeRepository repository, IMapper mapper) : 
        base(repository, mapper)
    {
        
    }
    public async Task<EmployeeResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await EmployeeRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ResourseNotFoundException("customer not found");
        }

        return Mapper.Map<EmployeeResponse>(customer);
    }
}