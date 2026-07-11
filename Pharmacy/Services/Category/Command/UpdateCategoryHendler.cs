using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Command;

public record UpdateCategoryCommand(long Id, CategoryRequest Request) : IRequest<CategoryResponse>;

public class UpdateCategoryHendler : CategoryDiBase, IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    public UpdateCategoryHendler(CategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await CategoryRepository.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new ResourseNotFoundException("Category not found");
        }

        var exixtCategory = await CategoryRepository.CategoryExistsAsync(request.Request.Name);
        if (exixtCategory)
        {
            throw new ResourseIsAlredyExistException("Category  with thia name alredy exsist");
        }

        Mapper.Map(request, category);
        await CategoryRepository.UpdateAsync(category);
        await CategoryRepository.SaveChangesAsync();
        return Mapper.Map<CategoryResponse>(request);
    }
}