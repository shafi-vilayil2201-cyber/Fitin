using Fitin.Application.Products.Interfaces;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.Products;
using Fitin.Application.Products.Dto;


namespace Fitin.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Include(x => x.Category)
            .Where(x => x.Category.Name == category)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsAsync(ProductQueryDto query)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query.Category))
        {
            products = products.Where(p => p.Category.Name == query.Category);
        }
        if (!string.IsNullOrEmpty(query.Sort))
        {
            switch (query.Sort.ToLower())
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
            }
        }
        return await products.ToListAsync();
    }
}
