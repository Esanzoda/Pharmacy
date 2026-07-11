using AutoMapper;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Category;

public class CategoryDiBase
{
   
    protected readonly ICategoryRepository CategoryRepository;
    protected readonly IMapper Mapper;

    public CategoryDiBase( ICategoryRepository categoryRepository,IMapper mapper)
    {
        Mapper = mapper;
        CategoryRepository = categoryRepository;
    }
    public CategoryDiBase( ICategoryRepository categoryRepository)
    {
        CategoryRepository = categoryRepository;
    }
   
}