using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Query;

public record GetAllCategoriesByPeginationQuery(int PageNumber, int PageSize) : IRequest<List<CategoryResponse>>;

public class GetAllCategoryHendler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCategoriesByPeginationQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetAllCategoriesByPeginationQuery request,
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