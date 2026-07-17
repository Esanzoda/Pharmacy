using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Query;

public record GetCategoryByIdQuery(long CategoryId) : IRequest<CategoryResponse>;

public class GetCategoryByIdHendler(
    IMapper mapper,
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"CategoryById-{request.CategoryId}";
        var cachedCategory = await cache.GetStringAsync(key, cancellationToken);
        if (cachedCategory is not null)
        {
            var entity = JsonConvert.DeserializeObject<Models.Domain.Category?>(cachedCategory);
            if (entity is not null)
            {
                return mapper.Map<CategoryResponse>(entity);
            }
        }

        var category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new ResourseNotFoundException("Category not found");
        }

        await cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(category), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }, cancellationToken);
        return mapper.Map<CategoryResponse>(category);
    }
}