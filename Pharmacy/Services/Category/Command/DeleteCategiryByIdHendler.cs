using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Command;

public record DeleteCategoryCommand(long Id) : IRequest<bool>;

public class DeleteCategiryByIdHendler : CategoryDiBase,IRequestHandler<DeleteCategoryCommand, bool>
{
   

    public DeleteCategiryByIdHendler(ICategoryRepository categoryRepository) : base(categoryRepository)
    {
       
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await CategoryRepository.DeleteAsync(request.Id);
        if (category is false)
        {
            throw new ResourseNotFoundException("Category not found  ");
        }

        await CategoryRepository.SaveChangesAsync();
        return category;
    }
}