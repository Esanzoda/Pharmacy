using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Command;

public record UpdateCategoryCommand(long Id, UpdateCategoryRequest Request) : IRequest<UpdateCategoryResponse>;

public class UpdateCategoryHendler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IDistributedCache cache)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new ResourseNotFoundException("Category not found");
        }

        var exixtCategory = await dbContext.Categories
            .AnyAsync(x => x.Name == request.Request.Name, cancellationToken);
        if (exixtCategory)
        {
            throw new ResourseIsAlredyExistException("Category  with this name alredy exsist");
        }

        mapper.Map(request.Request, category);
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        var key = $"CategoryById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return mapper.Map<UpdateCategoryResponse>(category);
    }
}