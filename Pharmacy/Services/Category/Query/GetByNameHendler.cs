using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Query;

public record GetByNameQuery(string Name) : IRequest<List<CategoryResponse>>;

public class GetByNameHendler(
    IMapper mapper,
    IApplicationDbContext dbContext) :
    IRequestHandler<GetByNameQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Where(x => x.Name == request.Name)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        if (!category.Any())
        {
            throw new ResourseNotFoundException("Category not found");
        }

        return mapper.Map<List<CategoryResponse>>(category);
    }
}