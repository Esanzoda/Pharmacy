using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetEmployeeByEmailQuery(string Email) : IRequest<EmployeeResponse>;
public class GetEmployeeByEmailHendler:EmployeDiBase,IRequestHandler<GetEmployeeByEmailQuery,EmployeeResponse>
{
    public GetEmployeeByEmailHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
    }

    public async Task<EmployeeResponse> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
    {
        var employee = await EmployeeRepository.GetEmployeeByEmailAsync(request.Email);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee whith this email {request.Email} not found ");
        }

        return Mapper.Map<EmployeeResponse>(employee); 
    }
}