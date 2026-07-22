using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Category.Queries;

public record GetByNameQuery(
    string Name) : IRequest<List<CategoryResponse>>;

public class GetByNameHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) :
    IRequestHandler<GetByNameQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Where(x => x.Name.Contains(request.Name))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        if (!category.Any())
        {
            throw new ResourseNotFoundException("Category not found");
        }

        return mapper.Map<List<CategoryResponse>>(category);
    }
}