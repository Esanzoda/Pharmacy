using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;
public record GetEmpoyeesByAddressQuery(string Address,int Page,int PageSize) : IRequest<List<EmployeeResponse>>;
public class GEtEmployeeByAddressHendler:EmployeDiBase,IRequestHandler<GetEmpoyeesByAddressQuery,List<EmployeeResponse>>
{
    public GEtEmployeeByAddressHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmpoyeesByAddressQuery request, CancellationToken cancellationToken)
    {
        var employees = await EmployeeRepository.GetEmployeesByAddressAsync(request.Address, request.Page, request.PageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee with this address{request.Address} not found ");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}