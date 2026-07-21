using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Category.Queries;

public record GetActiveCategoriesQuery(
    int PageNumber,
    int PageSize) : IRequest<List<CategoryResponse>>;

public class GetActiveCategoriesHandler(
    IMapper mapper,
    IApplicationDbContext dbContext):
        IRequestHandler<GetActiveCategoriesQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetActiveCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Where(x => x.CategoryStatus == CategoryStatus.Active)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber-1)*request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return mapper.Map<List<CategoryResponse>>(category);
    }
}