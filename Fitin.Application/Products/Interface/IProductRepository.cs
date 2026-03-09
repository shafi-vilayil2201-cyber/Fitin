using Fitin.Domain.Entities.Product;


namespace Fitin.Application.Products.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync (Product product);
    Task UpdateAsync (Product product);
    Task DeleteAsync (Product product);

}
