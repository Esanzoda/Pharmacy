using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByEmailQuery(
    string Email) : IRequest<EmployeeResponse>;

public class GetEmployeeByEmailHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetEmployeeByEmailQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.Email!.ToLower() == request.Email.ToLower(), cancellationToken);
        if (employee == null)
        {
            throw new RecourseNotFoundException($"Employee with this email {request.Email} not found ");
        }

        return mapper.Map<EmployeeResponse>(employee);
    }
}