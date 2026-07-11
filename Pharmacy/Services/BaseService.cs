using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface IBaseService<TRequest, TResponse>
{
    Task<TResponse> CreateAsync(TRequest request);
    Task<TResponse> UpdateAsync(long id, TRequest request);
    Task<TResponse?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<TResponse>> GetAllByPaginationAsync(int pageNumber, int pageSize);
    Task<bool> DeleteAsync(long id);
}

public class BaseService<TEntity, TRequest, TResponse> : IBaseService<TRequest, TResponse> where TEntity : class
{
    protected readonly IBaseRepository<TEntity> BaseRepository;
    protected readonly IMapper Mapper;
    private readonly IDistributedCache _cache;

    public BaseService(IBaseRepository<TEntity> repository,
        IMapper mapper, IDistributedCache distributedCache)
    {
        BaseRepository = repository;
        Mapper = mapper;
        _cache = distributedCache;
    }
    

    public virtual async Task<TResponse> CreateAsync(TRequest request)
    {
        var entity = Mapper.Map<TEntity>(request);
        await BaseRepository.CreateAsync(entity);
        await BaseRepository.SaveChangesAsync();
        return Mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> UpdateAsync(long id, TRequest request)
    {
        var key = $"{typeof(TEntity).Name}-{id}";
        var cached = await _cache.GetStringAsync(key);
        if (cached != null)
        {
            var entity = JsonConvert.DeserializeObject<TEntity>(cached);
            Mapper.Map(request, entity);
            var updateEntity = await BaseRepository.UpdateAsync(entity);
            await BaseRepository.SaveChangesAsync();
            return Mapper.Map<TResponse>(updateEntity);
        }
        else
        {

            var entity = await BaseRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new ResourseNotFoundException($"{typeof(TEntity).Name} with id {id} not found");
            }

            await _cache.SetStringAsync(
                key, JsonConvert.SerializeObject(entity), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            Mapper.Map(request, entity);
            var updateEntity = await BaseRepository.UpdateAsync(entity);
            await BaseRepository.SaveChangesAsync();
            return Mapper.Map<TResponse>(updateEntity);
        }
    }


public async Task<TResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = $"{typeof(TEntity).Name}-{id}";
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        TEntity? entity;
        if (cached != null)
        {
           entity = JsonConvert.DeserializeObject<TEntity?>(cached);
            return Mapper.Map<TResponse>(entity);
        }

        entity = await BaseRepository.GetByIdAsync(id);
        if (entity is null)
        {
            return default;
        }

        await _cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(entity), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        return Mapper.Map<TResponse>(entity);


    }

    public async Task<List<TResponse>> GetAllByPaginationAsync(int pageNumber, int pageSize)
    {
        var entities = await BaseRepository.GetAllByPaginationAsync(pageNumber, pageSize);
        return Mapper.Map<List<TResponse>>(entities);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var delete = await BaseRepository.DeleteAsync(id);
        if (delete == false)
        {
            throw new ResourseNotFoundException($"{typeof(TEntity).Name} with id {id} not found");
        }
        else
        {
            await BaseRepository.SaveChangesAsync();
            return delete;
        }
    }
}