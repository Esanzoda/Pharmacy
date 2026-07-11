using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IEmployeeService
    : IBaseService<EmployeeRequest, EmployeeResponse>
{
    Task<List<EmployeeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize);
    Task<List<EmployeeResponse>> GetEmployeesByAdressAsync(string adress, int page, int pageSize);
    Task<EmployeeResponse> GetEmployeesByNumberAsync(string number);
    Task<EmployeeResponse> GetEmployeeByEmailAsync(string email);
    Task<List<EmployeeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize);
    Task<List<EmployeeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize);
}

public class EmployeeService : BaseService<Models.Domain.Employee, EmployeeRequest, EmployeeResponse>,
    IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
   private readonly IDistributedCache _cache;

    public EmployeeService(IEmployeeRepository employeRepository, IMapper mapper,IDistributedCache distributedCache)
        : base(employeRepository, mapper,distributedCache)
    {
        _employeeRepository = employeRepository;
        _cache = distributedCache;
    }

    public async override Task<EmployeeResponse> CreateAsync(EmployeeRequest request)
    {
        var employeeByEmail = await _employeeRepository.GetEmployeeByEmailAsync(request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExistException($"Email {request.Email} already exists");
        }

        var employeeByPhone = await _employeeRepository.GetEmployeesByNumberAsync(request.PhoneNumber);
        if (employeeByPhone != null)
        {
            throw new ResourseIsAlredyExistException($"Phone {request.PhoneNumber} already exists");
        }

        if (request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        var employee = Mapper.Map<Models.Domain.Employee>(request);
        await _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return Mapper.Map<EmployeeResponse>(employee);
    }

    public async override Task<EmployeeResponse> UpdateAsync(long id, EmployeeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee with id {id} not found");
        }

        var employeeByEmail = await _employeeRepository.GetEmployeeByEmailAsync(request.Email);
        if (employeeByEmail != null)
        {
            throw new ResourseIsAlredyExistException($"Email {request.Email} already exists");
        }

        var employeByPhone = await _employeeRepository.GetEmployeesByNumberAsync(request.PhoneNumber);
        if (employeByPhone != null)
        {
            throw new ResourseIsAlredyExistException($"Phone {request.PhoneNumber} already exists");
        }

        if (request.Role == Role.Customer)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        Mapper.Map(request, employee);
        await _employeeRepository.UpdateAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return Mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<List<EmployeeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByNameAsync(name, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this name {name} not found ");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }

    public async Task<List<EmployeeResponse>> GetEmployeesByAdressAsync(string address, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByAddressAsync(address, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee with this address{address} not found ");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }

    public async Task<EmployeeResponse> GetEmployeesByNumberAsync(string number)
    {
        var employees = await _employeeRepository.GetEmployeesByNumberAsync(number);

        if (employees == null)
        {
            throw new ResourseNotFoundException($"Employee whith this number {number}  not found ");
        }

        return Mapper.Map<EmployeeResponse>(employees);
    }

    public async Task<EmployeeResponse> GetEmployeeByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetEmployeeByEmailAsync(email);
        if (employee == null)
        {
            throw new ResourseNotFoundException($"Employee whith this email {email} not found ");
        }

        return Mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<List<EmployeeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesBySalaryAsync(salary, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this salary {salary} not found");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }

    public async Task<List<EmployeeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetAllEmployeeByRoleAsync(role, page, pageSize);

        if (!employees.Any())
        {
            throw new ResourseNotFoundException($"Employee whith this role {role} not found");
        }

        return Mapper.Map<List<EmployeeResponse>>(employees);
    }
}