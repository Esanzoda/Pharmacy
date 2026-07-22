using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetAllEmployeeByPagenationQuery(
    int PageNumber,
    int PageSize) : IRequest<List<EmployeeResponse>>;

public class GetAllEmployeeByPagenationQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetAllEmployeeByPagenationQuery, List<EmployeeResponse>>
{
    public async Task<List<EmployeeResponse>> Handle(GetAllEmployeeByPagenationQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return mapper.Map<List<EmployeeResponse>>(employees);
    }
}