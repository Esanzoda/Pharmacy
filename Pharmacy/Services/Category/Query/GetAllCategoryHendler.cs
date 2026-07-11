using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category.Query;

public record GetAllCategoriesByPeginationQuery(int PageNumber, int PageSize) : IRequest<List<CategoryResponse>>;

public class GetAllCategoryHendler : CategoryDiBase,IRequestHandler<GetAllCategoriesByPeginationQuery, List<CategoryResponse>>
{
   

    public GetAllCategoryHendler(ICategoryRepository categoryRepository, IMapper mapper)
        : base(categoryRepository, mapper)
    {
        
    }

    public async Task<List<CategoryResponse>> Handle(GetAllCategoriesByPeginationQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await CategoryRepository.GetAllByPaginationAsync(request.PageNumber, request.PageSize);
        return Mapper.Map<List<CategoryResponse>>(entities);
    }
}