using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Employee.Queries;

public record GetEmpoyeesByNumberQuery(
    string Number
) : IRequest<EmployeeResponse>;

public class GetEmployeeByNumberQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetEmpoyeesByNumberQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmpoyeesByNumberQuery request, CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.Number, cancellationToken);
        if (employees == null)
        {
            throw new ResourseNotFoundException($"Employee whith this number {request.Number}  not found ");
        }

        return mapper.Map<EmployeeResponse>(employees);
    }
}