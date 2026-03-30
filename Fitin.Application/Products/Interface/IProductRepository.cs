using Fitin.Application.Common.Interfaces;
using Fitin.Application.Products.Dto;
using Fitin.Domain.Entities.Products;

namespace Fitin.Application.Products.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    new Task<Product?> GetByIdAsync(Guid id);
    new Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetProductsAsync(ProductQueryDto query);
}
