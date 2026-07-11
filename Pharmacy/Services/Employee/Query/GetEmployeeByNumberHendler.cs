using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetEmpoyeesByNumberQuery(string Number):IRequest<EmployeeResponse>;
public class GetEmployeeByNumberHendler:EmployeDiBase,IRequestHandler<GetEmpoyeesByNumberQuery,EmployeeResponse>
{
    public GetEmployeeByNumberHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
    }

    public async Task<EmployeeResponse> Handle(GetEmpoyeesByNumberQuery request, CancellationToken cancellationToken)
    {
        var employees = await EmployeeRepository.GetEmployeesByNumberAsync(request.Number);

        if (employees == null)
        {
            throw new ResourseNotFoundException($"Employee whith this number {request.Number}  not found ");
        }

        return Mapper.Map<EmployeeResponse>(employees);
    }
}