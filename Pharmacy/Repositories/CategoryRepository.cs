using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<Product>> GetCategoryWithProducts(int categoryId, int page, int pageSize);
    Task<List<Category>> SearchByNameAsync(string name);
    Task<List<Category>> GetActiveCategoriesAsync();
    Task<bool> CategoryExistsAsync(string name);
}

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Product>> GetCategoryWithProducts(int categoryId, int page, int pageSize)
    {
        return await DbContext.Products
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x=>x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Category>> SearchByNameAsync(string name)
    {
        return await DbContext.Categories
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<Category>> GetActiveCategoriesAsync()
    {
        return await DbContext.Categories
            .Where(x => x.CategoryStatus == CategoryStatus.Active)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<bool> CategoryExistsAsync(string name)
    {
        return await DbContext.Categories
            .AnyAsync(x => x.Name.ToLower() == name.ToLower());
    }
}