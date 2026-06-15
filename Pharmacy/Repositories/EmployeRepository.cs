using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<List<Employee>> GetEmployeesByNameAsync(string name, int page, int pageSize);    
    Task<List<Employee>> GetEmployeesByAdressAsync(string adress, int page, int pageSize);    
    Task<List<Employee>> GetEmployeesByNumberAsync(string number,int page, int pageSize);    
    Task<Employee?> GetEmployeeByEmailAsync(string email);    
    Task<List<Employee>> GetEmployeesBySalaryAsync(decimal salary,int page, int pageSize); 
    Task<List<Employee>> GetAllEmployeeByRoleAsync(Role role,int page, int pageSize); 
}
public class EmployeeRepository:BaseRepository<Employee>,IEmployeeRepository
{
    public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<Employee>> GetEmployeesByNameAsync(string name, int page, int pageSize)
    {
        return _dbSet
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Employee>> GetEmployeesByAdressAsync(string adress, int page, int pageSize)
    {
        return _dbSet
            .Where(x => x.Address.ToLower().Contains(adress.ToLower()))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<Employee>> GetEmployeesByNumberAsync(string number, int page, int pageSize)
    {
        return _dbSet
            .Where(x=>x.PhonNumber==number)
            .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        return await _dbContext.Employees
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public Task<List<Employee>> GetEmployeesBySalaryAsync(decimal salary, int page, int pageSize)
    {
        return _dbSet
            .Where(x => x.Salary ==salary)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
   
 
    public Task<List<Employee>> GetAllEmployeeByRoleAsync(Role role, int page, int pageSize)
    {
        return _dbSet
            .Where(x => x.Role==role)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}