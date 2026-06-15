using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICategoryServise :
    IBaseService< CategoryRequest, CategoryResponse>
{
    Task<List<ProductResponse>> GetCategoryWithProducts(int categoryId,int page , int pageSize);
    Task<List<CategoryResponse>>SearchByNameAsync(string name);
    Task<List<CategoryResponse>> GetActiveCategoriesAsync();
    Task<bool> CategoryExistsAsync(string name);
}

public class CategoryService:BaseService<Category,CategoryRequest,CategoryResponse>,
    ICategoryServise
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) 
        : base(categoryRepository, mapper)
    {
        _categoryRepository = categoryRepository;
    }

    public async override Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        if (await _categoryRepository.CategoryExistsAsync(request.Name))
             throw new ResourseIsAlredyExsistExeption("Category already exists");

        var category=_mapper.Map<Category>(request);
        category.CreateAt = DateTime.UtcNow;
        category.CategoryStatus = CategoryStatus.Active;
        category=await _categoryRepository.CreateAsync(category);
        return _mapper.Map<CategoryResponse>(category);
         
    }

    public async Task<List<ProductResponse>> GetCategoryWithProducts(int categoryId,int page, int pageSize)
    { 
        var products =await _categoryRepository.GetCategoryWithProducts(categoryId,page,pageSize);
        if (!products.Any() )
            throw new ResourseNotFoundExeption("No categories found with this id");
        
        
        return  _mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<List<CategoryResponse>> SearchByNameAsync(string name)
    {
        var categories =await _categoryRepository.SearchByNameAsync(name);
        if (!categories.Any())
            throw new ResourseNotFoundExeption("No categories found with this name");
        
        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<List<CategoryResponse>> GetActiveCategoriesAsync()
    {
        var categories = _categoryRepository.GetActiveCategoriesAsync();
        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        
        return await _categoryRepository.CategoryExistsAsync(name);
    }
}