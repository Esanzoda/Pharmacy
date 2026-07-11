using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Role = Pharmasy.Models.Domain.Enum.Role;

namespace Pharmasy.Services.Employee.Query;
public record GetEmployeeByRoleQuery(Role Role,int Page,int PageSize) : IRequest<List<EmployeeResponse>>;
public class GetEmployeeByRoleHendler:EmployeDiBase,IRequestHandler<GetEmployeeByRoleQuery,List<EmployeeResponse>>
{
    public GetEmployeeByRoleHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmployeeByRoleQuery request, CancellationToken cancellationToken)
    {
        var employees = await EmployeeRepository.GetAllEmployeeByRoleAsync(request.Role, request.Page, request.PageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this role {request.Role} not found");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}