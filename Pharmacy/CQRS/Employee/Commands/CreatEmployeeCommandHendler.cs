using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Commands;

public record CreateEmployeeCommand(
    EmployeeRequest Request) : IRequest<EmployeeResponse>;

public class CreatEmployeeCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeExist = await dbContext.Employees
            .AnyAsync(x => x.Email!.ToLower() == request.Request.Email.ToLower()
                           || x.Name == request.Request.PhoneNumber,
                cancellationToken);
        if (employeeExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Email: {request.Request.Email} or Number{request.Request.PhoneNumber}already exists");
        }

        if (request.Request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        var newEmployee = mapper.Map<Models.Domain.Employee>(request.Request);
        await dbContext.Employees.AddAsync(newEmployee, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<EmployeeResponse>(newEmployee);
    }
}