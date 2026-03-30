using Fitin.Application.Products.Dto;

namespace Fitin.Application.Products.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(Guid id);

    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto);

    Task DeleteAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsAsync(ProductQueryDto query);
}
