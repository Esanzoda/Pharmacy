using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface ICustomerRepository: IBaseRepository<Customer>
{
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<Customer?> GetCustomerByPhoneAsync(string phone);
    Task<List<Customer>> GetCustomerByNameAsync(string name);
}
public class  CustomerRepository:BaseRepository<Customer>,ICustomerRepository
    
{
    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return DbContext.Customers
            .FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
    {
        return  DbContext.Customers
            .FirstOrDefault(x => x.PhoneNumber == phone);
    }

    public async Task<List<Customer>> GetCustomerByNameAsync(string name)
    {
        return await DbContext.Customers
            .Where(x => x.Name.ToLower()==name.ToLower())
            .ToListAsync();
    }
    
   
}