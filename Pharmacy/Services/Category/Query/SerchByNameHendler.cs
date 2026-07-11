using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Query;

public record SerchByNameQuery(string Name) : IRequest<List<CategoryResponse>>;

public class SerchByNameHendler : CategoryDiBase, IRequestHandler<SerchByNameQuery, List<CategoryResponse>>
{
    public SerchByNameHendler(ICategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
    }

    public async Task<List<CategoryResponse>> Handle(SerchByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await CategoryRepository.SearchByNameAsync(request.Name);
        if (!category.Any())
        {
            throw new ResourseNotFoundException("Category not found");
        }

        return Mapper.Map<List<CategoryResponse>>(category);
    }
}