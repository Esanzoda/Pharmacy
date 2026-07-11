using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICategoryServise :
    IBaseService<CategoryRequest, CategoryResponse>
{
    Task<List<ProductResponse>> GetCategoryWithProducts(int categoryId, int page, int pageSize);
    Task<List<CategoryResponse>> SearchByNameAsync(string name);
    Task<List<CategoryResponse>> GetActiveCategoriesAsync();
    Task<bool> CategoryExistsAsync(string name);
}

public class CategoryService : BaseService<Models.Domain.Category, CategoryRequest, CategoryResponse>,
    ICategoryServise
{
    private readonly ICategoryRepository _categoryRepository;
   
    

    public CategoryService(ICategoryRepository categoryRepository,  IDistributedCache distributedCache,IMapper mapper)
        : base(categoryRepository,mapper, distributedCache)
    {
        
       
    }

    public async override Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        if (await _categoryRepository.CategoryExistsAsync(request.Name))
        {
            throw new ResourseIsAlredyExistException("Category already exists");
        }

        var category = Mapper.Map<Models.Domain.Category>(request);
        category.CategoryStatus = CategoryStatus.Active;
        category = await _categoryRepository.CreateAsync(category);
        await _categoryRepository.SaveChangesAsync();
        return Mapper.Map<CategoryResponse>(category);
    }

    public async override Task<CategoryResponse> UpdateAsync(long id, CategoryRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new ResourseNotFoundException("Category not found");
        }

        var exixtCategory = await _categoryRepository.CategoryExistsAsync(request.Name);
        if (exixtCategory)
        {
             throw new ResourseIsAlredyExistException("Category  with thia name alredy exsist");
        }

        Mapper.Map(request, category);
        await _categoryRepository.UpdateAsync(category);
        await _categoryRepository.SaveChangesAsync();
        return Mapper.Map<CategoryResponse>(category);
    }

    public async Task<List<ProductResponse>> GetCategoryWithProducts(int categoryId, int page, int pageSize)
    {
        var products = await _categoryRepository.GetCategoryWithProducts(categoryId, page, pageSize);
        if (!products.Any())
        {
            throw new ResourseNotFoundException("In this category doesnt exsist products ");
        }

        return Mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<List<CategoryResponse>> SearchByNameAsync(string name)
    {
        var categories = await _categoryRepository.SearchByNameAsync(name);
        if (!categories.Any())
        {
            throw new ResourseNotFoundException("Category not found ");
        }

        return Mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<List<CategoryResponse>> GetActiveCategoriesAsync()
    {
        var categories = await _categoryRepository.GetActiveCategoriesAsync();
        if (!categories.Any())
        {
            throw new ResourseNotFoundException("Acktive Category not found");
        }

        return Mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await _categoryRepository.CategoryExistsAsync(name);
    }
}