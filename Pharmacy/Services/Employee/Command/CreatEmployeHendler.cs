using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Command;

public record CreateEmployeCommand(EmployeeRequest Request) : IRequest<EmployeeResponse>;

public class CreatEmployeHendler : EmployeDiBase, IRequestHandler<CreateEmployeCommand, EmployeeResponse>
{
    public CreatEmployeHendler(IMapper mapper, IEmployeeRepository employeeRepository)
        : base(employeeRepository, mapper)
    {
    }

    public async Task<EmployeeResponse> Handle(CreateEmployeCommand request, CancellationToken cancellationToken)
    {
        var employeeByEmail = await EmployeeRepository.GetEmployeeByEmailAsync(request.Request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExistException($"Email {request.Request.Email} already exists");
        }

        var employeeByPhone = await EmployeeRepository.GetEmployeesByNumberAsync(request.Request.PhoneNumber);
        if (employeeByPhone != null)
        {
            throw new ResourseIsAlredyExistException($"Phone {request.Request.PhoneNumber} already exists");
        }

        if (request.Request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        var employee = Mapper.Map<Models.Domain.Employee>(request.Request);
        await EmployeeRepository.CreateAsync(employee);
        await EmployeeRepository.SaveChangesAsync();
        return Mapper.Map<EmployeeResponse>(employee);
    }
}