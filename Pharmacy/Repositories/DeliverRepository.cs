using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IDeliverRepository : IBaseRepository<Deliver>
{
    Task<Deliver?> GetDeliverByEmail(string email);
    Task<Deliver?> GetDeliverByPhoneAsync(string phone);
}

public class DeliverRepository : BaseRepository<Deliver>, IDeliverRepository
{
    public DeliverRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<Deliver?> GetDeliverByEmail(string email)
    {
        return await DbContext.Delivers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
    public async Task<Deliver?> GetDeliverByPhoneAsync(string phone)
    {
        return await DbContext.Delivers
            .FirstOrDefaultAsync(x => x.PhoneNumber == phone);
    }
}