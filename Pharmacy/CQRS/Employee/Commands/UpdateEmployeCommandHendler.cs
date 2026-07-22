using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Commands;

public record UpdateEmployeeCommand(
    long Id,
    EmployeeRequest Request
) : IRequest<EmployeeResponse>;

public class UpdateEmployeHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FindAsync(request.Id, cancellationToken);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with id {request.Id} not found");
        }

        var employeeExist = await dbContext.Employees
            .AnyAsync(x => x.Id != request.Id && 
                           (x.Email == request.Request.Email
                           || x.PhoneNumber == request.Request.PhoneNumber),
                cancellationToken);

        if (employeeExist)
        {
            throw new ResourseIsAlredyExistException(
                $"Email: {request.Request.Email} or Number{request.Request.PhoneNumber}already exists");
        }

        if (request.Request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        mapper.Map(request.Request, employee);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<EmployeeResponse>(employee);
    }
}