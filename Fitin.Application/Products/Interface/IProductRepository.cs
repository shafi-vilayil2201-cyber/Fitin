using Fitin.Application.Common.Interfaces;
using Fitin.Application.Products.Dto;
using Fitin.Domain.Entities.Products;


namespace Fitin.Application.Products.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetProductsAsync(ProductQueryDto query);
}
