using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Query;

public record GetAllEmployeeByPagenationQuery(int PageNumber, int PageSize) : IRequest<List<EmployeeResponse>>;
public class GetAllEmployeeByPagenationHendler:EmployeDiBase,IRequestHandler<GetAllEmployeeByPagenationQuery,List<EmployeeResponse>>
{
    public GetAllEmployeeByPagenationHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
        
    }
    public async Task<List<EmployeeResponse>> Handle(GetAllEmployeeByPagenationQuery request, CancellationToken cancellationToken)
    {
        var employees = EmployeeRepository.GetAllByPaginationAsync(request.PageNumber, request.PageSize);
        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}