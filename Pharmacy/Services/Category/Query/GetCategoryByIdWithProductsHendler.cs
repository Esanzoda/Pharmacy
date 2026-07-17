using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Query;

public record GetCategoryByIdWithProductsQuery(int CategoryId, int PageNumber, int PageSize)
    : IRequest<List<CategoryResponse>>;

public class GetCategoryByIdWithProductsHendler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    :
        IRequestHandler<GetCategoryByIdWithProductsQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetCategoryByIdWithProductsQuery request,
        CancellationToken cancellationToken)
    {
        var category =
         await dbContext.Products
            .Where(x => x.CategoryId == request.CategoryId)
            .OrderBy(x=>x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (!category.Any())
        {
            throw new ResourseNotFoundException("In this category doesnt exsist products ");
        }

        return mapper.Map<List<CategoryResponse>>(category);
    }
}