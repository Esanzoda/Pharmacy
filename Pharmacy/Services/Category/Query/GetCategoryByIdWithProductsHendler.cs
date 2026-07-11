using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Query;

public record GetCategoryByIdWithProductsQuery(int CategoryId, int PageNumber, int PageSize)
    : IRequest<List<CategoryResponse>>;

public class GetCategoryByIdWithProductsHendler : CategoryDiBase,
    IRequestHandler<GetCategoryByIdWithProductsQuery, List<CategoryResponse>>
{
    public GetCategoryByIdWithProductsHendler(ICategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoryByIdWithProductsQuery request,
        CancellationToken cancellationToken)
    {
        var category =
            await CategoryRepository.GetCategoryWithProducts(request.CategoryId, request.PageNumber, request.PageSize);
        if (!category.Any())
        {
            throw new ResourseNotFoundException("In this category doesnt exsist products ");
        }

        return Mapper.Map<List<CategoryResponse>>(category);
    }
}