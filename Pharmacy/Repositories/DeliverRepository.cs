using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;

public interface IDeliverRepository : IBaseRepository<Deliver>
{
}

public class DeliverRepository : BaseRepository<Deliver>, IDeliverRepository
{
    public DeliverRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}