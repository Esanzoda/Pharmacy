using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Interfaces;

namespace Pharmasy.CQRS.Category.Commands;

public record DeleteCategoryCommand(
    long Id) : IRequest<bool>;

public class DeleteCategoryByIdHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (category is null)
        {
            throw new ResourseNotFoundException("Category not found  ");
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"CustomerById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);
        return true;
    }
}