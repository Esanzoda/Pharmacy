using Microsoft.EntityFrameworkCore;
using Pharmasy.Data;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Repositories;

public class ExpiryDateProductRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<ExpiryDateProduct> _dbSet;
    private readonly IProductRepository _productRepository;

    public ExpiryDateProductRepository(ILogger<ExpiryDateProductRepository> logger, DbSet<ExpiryDateProduct> dbSet,
        AppDbContext dbContext, IProductRepository productRepository)
    {
        _dbContext = dbContext;
        _productRepository = productRepository;
        _dbSet = dbSet;
    }
/*
    public async Task<ExpiryDateResponse> GetExpiryDateProduct()
    {
        var expirydate = await _productRepository
                .GetExpiryDateAsync();

    }*/
}