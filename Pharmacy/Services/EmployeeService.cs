using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IEmployeeService
    : IBaseService< EmployeRequest, EmployeResponse>
{
    Task<List<EmployeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize);
    Task<List<EmployeResponse>> GetEmployeesByAdressAsync(string adress, int page, int pageSize);
    Task<List<EmployeResponse>> GetEmployeesByNumberAsync(string number, int page, int pageSize);
    Task<EmployeResponse> GetEmployeeByEmailAsync(string email);
    Task<List<EmployeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize);
    Task<List<EmployeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize);
  
}
public class EmployeeService:BaseService<Employee,EmployeRequest,EmployeResponse>,
    IEmployeeService 
{
    private readonly IEmployeeRepository _employeeRepository;
     public EmployeeService(IEmployeeRepository employeRepository, IMapper mapper) 
        : base(employeRepository, mapper)
    {
        _employeeRepository = employeRepository;
    }

    public async Task<List<EmployeResponse>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByNameAsync(name, page, pageSize);

        if (!employees.Any())
            throw new Exception("No employees found with this name");

        return _mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<List<EmployeResponse>> GetEmployeesByAdressAsync(string adress, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByAdressAsync(adress, page, pageSize);

        if (!employees.Any())
            throw new Exception("No employees found with this address");

        return _mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<List<EmployeResponse>> GetEmployeesByNumberAsync(string number, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesByNumberAsync(number, page, pageSize);

        if (!employees.Any())
            throw new Exception("No employees found with this number");

        return _mapper.Map<List<EmployeResponse>>(employees); 
    }

    public async Task<EmployeResponse> GetEmployeeByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetEmployeeByEmailAsync(email);

        if (employee == null)
            throw new Exception("Employee not found");

        return _mapper.Map<EmployeResponse>(employee);
    }

    public async Task<List<EmployeResponse>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetEmployeesBySalaryAsync(salary, page, pageSize);

        if (!employees.Any())
            throw new Exception("No employees found with this salary");

        return _mapper.Map<List<EmployeResponse>>(employees);
    }

    public async Task<List<EmployeResponse>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        var employees = await _employeeRepository.GetAllEmployeeByRoleAsync(role, page, pageSize);

        if (!employees.Any())
            throw new Exception("No employees found with this role");

        return _mapper.Map<List<EmployeResponse>>(employees);
    }
}