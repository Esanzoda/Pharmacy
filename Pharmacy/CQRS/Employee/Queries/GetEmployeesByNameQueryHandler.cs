using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmpoyeesByNameQuery(
    string Name,
    int Page,
    int PageSize
    ) : IRequest<List<EmployeeResponse>>;

public class GetEmployeesByNameQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
    ) : IRequestHandler<GetEmpoyeesByNameQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmpoyeesByNameQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.Name!.ToLower().Contains(request.Name.ToLower()))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);


        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this name {request.Name} not found ");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}