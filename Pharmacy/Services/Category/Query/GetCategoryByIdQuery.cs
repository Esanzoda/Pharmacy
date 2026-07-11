using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Query;

public record GetCategoryByIdQuery(long CategoryId) : IRequest<CategoryResponse>;

public class GetCategoryByIdHendler : CategoryDiBase, IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly IDistributedCache _cache;

    public GetCategoryByIdHendler(ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache)
        : base(categoryRepository, mapper)
    {
        _cache = cache;
    }


    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"{typeof(Models.Domain.Category).Name}-{request.CategoryId}";
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        Models.Domain.Category? entity;
        if (cached != null)
        {
            entity = JsonConvert.DeserializeObject<Models.Domain.Category?>(cached);
            return Mapper.Map<CategoryResponse>(entity);
        }

        entity = await CategoryRepository.GetByIdAsync(request.CategoryId);
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
        return Mapper.Map<CategoryResponse>(entity);
    }
}