
using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;

namespace Pharmasy.Repositories;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity request);
    Task<TEntity>UpdateAsync(TEntity request );
    Task<TEntity?> GetByIdAsync(long id);
    Task<List<TEntity>>GetAllByPagenationAsync(int pageNumber, int pageSize);
    Task<bool> DeleteAsync(long id);
}
public class BaseRepository<TEntity> :IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;
    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
         _dbSet.Add(entity);
        await  _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
       _dbSet.Update(entity);
       await _dbContext.SaveChangesAsync();
       return entity;
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await   _dbSet
            .FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllByPagenationAsync(int pageNumber, int pageSize)
    {
        return await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;
        
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}