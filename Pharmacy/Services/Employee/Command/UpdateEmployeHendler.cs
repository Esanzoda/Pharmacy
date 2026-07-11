using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Command;

public record UpdateEmployeCommand(long Id, EmployeeRequest Request) : IRequest<EmployeeResponse>;
public class UpdateEmployeHendler:EmployeDiBase,IRequestHandler<UpdateEmployeCommand,EmployeeResponse>
{
   

    public UpdateEmployeHendler(IEmployeeRepository employeeRepository, IMapper mapper) 
        : base(employeeRepository, mapper)
    {
       
    }

    public async Task<EmployeeResponse> Handle(UpdateEmployeCommand request, CancellationToken cancellationToken)
    {
        var employee = await EmployeeRepository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with id {request.Id} not found");
        }

        var employeeByEmail = await EmployeeRepository.GetEmployeeByEmailAsync(request.Request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExistException($"Email {request.Request.Email} already exists");
        }

        var employeByPhone = await EmployeeRepository.GetEmployeesByNumberAsync(request.Request.PhoneNumber);
        if (employeByPhone != null)
        {
            throw new ResourseIsAlredyExistException($"Phone {request.Request.PhoneNumber} already exists");
        }

        if (request.Request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        Mapper.Map(request, employee);
        await EmployeeRepository.UpdateAsync(employee);
        await EmployeeRepository.SaveChangesAsync();
        return Mapper.Map<EmployeeResponse>(employee);
    }
}