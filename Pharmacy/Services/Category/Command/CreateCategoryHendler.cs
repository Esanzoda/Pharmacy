using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Command;

public record CreateCategoryCommand(CategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryHendler : CategoryDiBase, IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public CreateCategoryHendler(ICategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await CategoryRepository.CategoryExistsAsync(request.Request.Name))
        {
            throw new ResourseIsAlredyExistException("Category already exist");
        }

        var category = Mapper.Map<Models.Domain.Category>(request.Request);
        category.CategoryStatus = CategoryStatus.Active;
        await CategoryRepository.CreateAsync(category);
        await CategoryRepository.SaveChangesAsync();
        return Mapper.Map<CategoryResponse>(category);
    }
}