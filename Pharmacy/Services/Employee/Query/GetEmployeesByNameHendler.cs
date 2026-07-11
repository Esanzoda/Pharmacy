using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetEmpoyeesByNameQuery(string Name, int Page, int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByNameHendler : EmployeDiBase, IRequestHandler<GetEmpoyeesByNameQuery, List<EmployeeResponse>>
{
    public GetEmployeesByNameHendler(IEmployeeRepository employeeRepository)
        : base(employeeRepository)
    {
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmpoyeesByNameQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await EmployeeRepository.GetEmployeesByNameAsync(request.Name, request.Page, request.PageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this name {request.Name} not found ");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}