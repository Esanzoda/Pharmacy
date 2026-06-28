using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IBaseService< TRequest,TResponse> 
{
    Task<TResponse> CreateAsync(TRequest request);
    Task<TResponse> UpdateAsync(long id,TRequest request);
    Task<TResponse> GetByIdAsync(long id);
    Task<List<TResponse>>GetAllByPaginationAsync(int  pageNumber, int pageSize);
    Task<bool> DeleteAsync(long id);
    
}
public class  BaseService<TEntity,TRequest,TResponse>:IBaseService<TRequest,TResponse> where TEntity : class
{
    protected readonly IBaseRepository<TEntity> BaseRepository;
    protected readonly IMapper Mapper;

    public BaseService(IBaseRepository<TEntity> repository,
        IMapper mapper)
    {
        BaseRepository = repository;
        Mapper = mapper;
    }
    public virtual async  Task<TResponse> CreateAsync(TRequest request)
    {
        var entity = Mapper.Map<TEntity>(request);
         await BaseRepository.CreateAsync(entity);
        return Mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> UpdateAsync(long id,TRequest request)
    {
        var entity = await BaseRepository.GetByIdAsync(id);
        if (entity == null)
            throw new ResourseNotFoundExeption($"{typeof(TEntity).Name} with id {id} not found");
    
        Mapper.Map(request, entity);
        var updateEntity = await BaseRepository.UpdateAsync(entity);
        return Mapper.Map<TResponse>(updateEntity);
        
    }

    public async Task<TResponse> GetByIdAsync(long id)
    {
        var entity = await BaseRepository.GetByIdAsync(id);
        if (entity == null)
            throw new ResourseNotFoundExeption($"{typeof(TEntity).Name}  with id {id} not found");
        
        return Mapper.Map<TResponse>(entity);
    }

    public  async Task<List<TResponse>> GetAllByPaginationAsync(int pageNumber, int pageSize)
    {
        var entities = await BaseRepository.GetAllByPagenationAsync(pageNumber, pageSize);
        return Mapper.Map<List<TResponse>>(entities);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await BaseRepository.DeleteAsync(id);
    }
    
}