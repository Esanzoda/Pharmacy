using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Category.Queries;

public record GetAllCategoriesByPaginationQuery(
    int PageNumber,
    int PageSize) : IRequest<List<CategoryResponse>>;

public class GetAllCategoryQueryHandler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCategoriesByPaginationQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetAllCategoriesByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // request.PageNumber = Math.Max(1, request.PageNumber);
        // request.PageSize = Math.Max(1, request.PageSize);
        var categories = await dbContext.Categories
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return mapper.Map<List<CategoryResponse>>(categories);
    }
}