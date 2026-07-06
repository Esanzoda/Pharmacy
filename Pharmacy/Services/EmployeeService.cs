using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IEmployeeService
    : IBaseService<EmployeRequest, EmployeResponse>
{
    Task<List<EmployeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize);
    Task<List<EmployeResponse>> GetEmployeesByAdressAsync(string adress, int page, int pageSize);
    Task<EmployeResponse> GetEmployeesByNumberAsync(string number);
    Task<EmployeResponse> GetEmployeeByEmailAsync(string email);
    Task<List<EmployeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize);
    Task<List<EmployeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize);
}

public class EmployeeService : BaseService<Employee, EmployeRequest, EmployeResponse>,
    IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeRepository, IMapper mapper)
        : base(employeRepository, mapper)
    {
        _employeeRepository = employeRepository;
    }

    public async override Task<EmployeResponse> CreateAsync(EmployeRequest request)
    {
        var employeeByEmail = await _employeeRepository.GetEmployeeByEmailAsync(request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Email {request.Email} already exists");
        }

        var employeeByPhone = await _employeeRepository.GetEmployeesByNumberAsync(request.PhoneNumber);
        if (employeeByPhone != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Phone {request.PhoneNumber} already exists");
        }

        if (request.Role == Role.Customer)
        {
            throw new BusinessExseption("Cant create employee with status customer");
        }

        var employee = Mapper.Map<Employee>(request);
        await _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SavechangesAsync();
        return Mapper.Map<EmployeResponse>(employee);
    }

    public async override Task<EmployeResponse> UpdateAsync(long id, EmployeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new ResourseNotFoundExeption($"Employee with id {id} not found");
        }

        var employeeByEmail = await _employeeRepository.GetEmployeeByEmailAsync(request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Email {request.Email} already exists");
        }

        var employeByPhone = await _employeeRepository.GetEmployeesByNumberAsync(request.PhoneNumber);
        if (employeByPhone != null)
        {
            throw new ResourseIsAlredyExsistExeption($"Phone {request.PhoneNumber} already exists");
        }

        if (request.Role == Role.Customer)
        {
            throw new BusinessExseption("Cant create employee with status customer");
        }

        Mapper.Map(request, employee);
        await _employeeRepository.UpdateAsync(employee);
        await _employeeRepository.SavechangesAsync();
        return Mapper.Map<EmployeResponse>(employee);
    }

    public async Task<List<EmployeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByNameAsync(name, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundExeption($"Employee whith this name {name} not found ");
        }

        return Mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<List<EmployeResponse>> GetEmployeesByAdressAsync(string address, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByAdressAsync(address, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundExeption($"Employee with this address{address} not found ");
        }

        return Mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<EmployeResponse> GetEmployeesByNumberAsync(string number)
    {
        var employees = await _employeeRepository.GetEmployeesByNumberAsync(number);

        if (employees == null)
        {
            throw new ResourseNotFoundExeption($"Employee whith this number {number}  not found ");
        }

        return Mapper.Map<EmployeResponse>(employees);
    }

    public async Task<EmployeResponse> GetEmployeeByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetEmployeeByEmailAsync(email);
        if (employee == null)
        {
            throw new ResourseNotFoundExeption($"Employee whith this email {email} not found ");
        }

        return Mapper.Map<EmployeResponse>(employee);
    }

    public async Task<List<EmployeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesBySalaryAsync(salary, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundExeption($"Employee whith this salary {salary} not found");
        }

        return Mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<List<EmployeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetAllEmployeeByRoleAsync(role, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundExeption($"Employee whith this role {role} not found");
        }

        return Mapper.Map<List<EmployeResponse>>(employees);
    }
}