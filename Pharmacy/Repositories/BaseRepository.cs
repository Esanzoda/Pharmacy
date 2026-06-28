
using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;

namespace Pharmasy.Repositories;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity request);
    Task<TEntity>UpdateAsync(TEntity request );
    Task<TEntity?> GetByIdAsync(long id);
    Task<List<TEntity>>GetAllByPagenationAsync(int pageNumber, int pageSize);
    Task<bool> DeleteAsync(long id);
}
public class  BaseRepository<TEntity> :IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;
    public BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
         DbSet.Add(entity);
        
        try
        {
            await  DbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("MAIN ERROR:");
            Console.WriteLine(ex.Message);

            Console.WriteLine("INNER ERROR:");
            Console.WriteLine(ex.InnerException?.Message);

            throw;
        }
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
       DbSet.Update(entity);
       await DbContext.SaveChangesAsync();
       return entity;
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await   DbSet
            .FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllByPagenationAsync(int pageNumber, int pageSize)
    {
        return await DbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
            return false;
        
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
        return true;
    }
}