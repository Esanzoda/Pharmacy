using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetEmployeeBySalaryQuery(decimal Salary,int Pagem,int PageSize) : IRequest<List<EmployeeResponse>>;
public class GetEmployeeBySalaryHendler:EmployeDiBase,IRequestHandler<GetEmployeeBySalaryQuery,List<EmployeeResponse>>
{
    public GetEmployeeBySalaryHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmployeeBySalaryQuery request, CancellationToken cancellationToken)
    {
        var employees = await EmployeeRepository.GetEmployeesBySalaryAsync(request.Salary, request.Pagem, request.PageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this salary {request.Salary} not found");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}