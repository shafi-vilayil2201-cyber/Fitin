using Fitin.Application.Products.Interfaces;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.Product;


namespace Fitin.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync();
    }
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

}
