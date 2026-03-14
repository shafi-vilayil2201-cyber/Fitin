using AutoMapper;
using Fitin.Domain.Entities.Products;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Products.Dto;

namespace Fitin.Application.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(string category)
    {
        var products = await _repository.GetByCategoryAsync(category);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);

        await _repository.AddAsync(product);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return null;

        _mapper.Map(dto, product);

        await _repository.UpdateAsync(product);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return;

        await _repository.DeleteAsync(product);
    }
    public async Task<IEnumerable<ProductDto?>> GetProductsAsync(ProductQueryDto query)
    {
        var products = await _repository.GetProductsAsync(query);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}