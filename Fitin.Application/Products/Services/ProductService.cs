using Fitin.Domain.Entities.Products;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Products.Dto;
using Fitin.Application.Common.Exceptions;
using Fitin.Application.Categories.Interface;

namespace Fitin.Application.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepo;
   

    public ProductService(
        IProductRepository repository,
        ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepo = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return products.Select(MapProduct);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return null;

        return MapProduct(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if(category == null)
            throw new NotFoundException("Category not found");
        
        var product = new Product(
            dto.Name,
            dto.Price,
            dto.CategoryId,
            dto.Stock,
            dto.ImageUrl,
            dto.Brand,
            dto.Sport,
            dto.Description,
            dto.ShortDescription,
            dto.LongDescription,
            dto.Rating,
            dto.Discount
        );

        await _repository.AddAsync(product);

        var createdProduct = await _repository.GetByIdAsync(product.Id);
        return MapProduct(createdProduct ?? product);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if(category == null)
            throw new NotFoundException("Category not found");

        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return null;

        product.UpdateDetails(
            dto.Name,
            dto.Price,
            dto.CategoryId,
            dto.Stock,
            dto.ImageUrl,
            dto.Brand,
            dto.Sport,
            dto.Description,
            dto.ShortDescription,
            dto.LongDescription,
            dto.Rating,
            dto.Discount);

        await _repository.UpdateAsync(product);

        var updatedProduct = await _repository.GetByIdAsync(id);
        return MapProduct(updatedProduct ?? product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return;

        await _repository.DeleteAsync(product);
    }
    public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductQueryDto query)
    {
        var products = await _repository.GetProductsAsync(query);
        return products.Select(MapProduct);
    }

    private static ProductDto MapProduct(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand,
            Sport = product.Sport,
            Description = product.Description,
            ShortDescription = product.ShortDescription,
            LongDescription = product.LongDescription,
            Rating = product.Rating,
            Discount = product.Discount
        };
    }
}
