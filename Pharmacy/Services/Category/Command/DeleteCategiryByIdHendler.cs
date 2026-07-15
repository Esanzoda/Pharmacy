using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Command;

public record DeleteCategoryCommand(long Id) : IRequest<bool>;

public class DeleteCategiryByIdHendler : CategoryDiBase,IRequestHandler<DeleteCategoryCommand, bool>
{
   private readonly IDistributedCache _cache;

    public DeleteCategiryByIdHendler(ICategoryRepository categoryRepository, IDistributedCache cache) : base(categoryRepository)
    {
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await CategoryRepository.DeleteAsync(request.Id);
        if (category is false)
        {
            throw new ResourseNotFoundException("Category not found  ");
        }
        await CategoryRepository.SaveChangesAsync();
        //delet category in redis
       var key = $"CustomerById-{request.Id}";
       await _cache.RemoveAsync(key, cancellationToken); 
        return category;
    }
}