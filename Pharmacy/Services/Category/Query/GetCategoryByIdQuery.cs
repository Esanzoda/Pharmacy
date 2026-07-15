using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
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
        var key = $"CategoryById-{request.CategoryId}";
        var cachedCategory = await _cache.GetStringAsync(key, cancellationToken);
        if (cachedCategory is not  null)
        {
            var entity = JsonConvert.DeserializeObject<Models.Domain.Category?>(cachedCategory);
            if (entity is not null)
            {
                return Mapper.Map<CategoryResponse>(entity);
            }
        }

        var category = await CategoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
        {
            throw new ResourseNotFoundException("Category not found");
        }

        await _cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(category), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        return Mapper.Map<CategoryResponse>(category);
    }
}