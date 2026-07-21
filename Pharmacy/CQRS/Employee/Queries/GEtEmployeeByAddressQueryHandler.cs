using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Employee.Queries;

public record GetEmpoyeesByAddressQuery(
    string Address,
    int Page,
    int PageSize
) : IRequest<List<EmployeeResponse>>;

public class GEtEmployeeByAddressQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetEmpoyeesByAddressQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetEmpoyeesByAddressQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .Where(x => x.Address!.ToLower() == request.Address.ToLower())
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        if (employees is null)
        {
            throw new ResourseNotFoundException($"Employee with this address{request.Address} not found ");
        }

        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}