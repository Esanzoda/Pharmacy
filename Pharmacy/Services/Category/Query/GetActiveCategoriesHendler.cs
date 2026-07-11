using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Query;

public record GetActiveCategoriesQuery : IRequest<List<CategoryResponse>>;

public class GetActiveCategoriesHendler : CategoryDiBase,
    IRequestHandler<GetActiveCategoriesQuery, List<CategoryResponse>>
{
    public GetActiveCategoriesHendler(ICategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
    }

    public async Task<List<CategoryResponse>> Handle(GetActiveCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var category = await CategoryRepository.GetActiveCategoriesAsync();
        return Mapper.Map<List<CategoryResponse>>(category);
    }
}