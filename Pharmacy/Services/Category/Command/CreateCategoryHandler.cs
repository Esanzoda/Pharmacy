using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Services.Category.Command;

public record CreateCategoryCommand(CreateCategoryRequest Request) 
    : IRequest<CategoryResponse>;

public class CreateCategoryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exsist = await dbContext.Categories
            .AnyAsync(x => x.Name == request.Request.Name, cancellationToken);

        if (exsist)
        {
            throw new ResourseIsAlredyExistException("Category alredy exist");
        }

        var category = mapper.Map<Models.Domain.Category>(request.Request);
        category.CategoryStatus = CategoryStatus.Active;
        await dbContext.Categories
            .AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<CategoryResponse>(category);
    }
}