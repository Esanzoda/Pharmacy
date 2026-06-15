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
public class BaseService<TEntity,TRequest,TResponse>:IBaseService<TRequest,TResponse> where TEntity : class
{
    protected readonly IBaseRepository<TEntity> _baseRepository;
    protected readonly IMapper _mapper;

    public BaseService(IBaseRepository<TEntity> repository,
        IMapper mapper)
    {
        _baseRepository = repository;
        _mapper = mapper;
    }
    public virtual async  Task<TResponse> CreateAsync(TRequest request)
    {
        var entity = _mapper.Map<TEntity>(request);
         await _baseRepository.CreateAsync(entity);
        return _mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> UpdateAsync(long id,TRequest request)
    {
        var entity = await _baseRepository.GetByIdAsync(id);
        if (entity == null)
            throw new ResourseNotFoundExeption($"Entity with id {id} not found");
    
        _mapper.Map(request, entity);
        var updateEntity = await _baseRepository.UpdateAsync(entity);
        return _mapper.Map<TResponse>(updateEntity);
        
    }

    public async Task<TResponse> GetByIdAsync(long id)
    {
        var entity = await _baseRepository.GetByIdAsync(id);
        if (entity == null)
            throw new ResourseNotFoundExeption($"Entity with id {id} not found");
        
        return _mapper.Map<TResponse>(entity);
    }

    public  async Task<List<TResponse>> GetAllByPaginationAsync(int pageNumber, int pageSize)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var entities = await _baseRepository.GetAllByPagenationAsync(pageNumber, pageSize);
        return _mapper.Map<List<TResponse>>(entities);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseRepository.DeleteAsync(id);
    }
    
}