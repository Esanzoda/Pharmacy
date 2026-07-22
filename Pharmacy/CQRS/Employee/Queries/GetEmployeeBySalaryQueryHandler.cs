using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeBySalaryQuery(
    decimal Salary,
    int Pagem,
    int PageSize
) : IRequest<List<EmployeeResponse>>;

public class GetEmployeeBySalaryQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmployeeBySalaryQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmployeeBySalaryQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.Salary == request.Salary)
            .OrderBy(x => x.Id)
            .Skip((request.Pagem - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this salary {request.Salary} not found");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}