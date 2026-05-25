using Fitin.Application.AI.DTOs;
using Fitin.Application.AI.Interfaces;
using Fitin.Application.Products.Interfaces;

namespace Fitin.Application.AI.Services;

public class ProductAssistantService : IProductAssistantService
{
    private readonly IProductService _productService;

    public ProductAssistantService(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<List<AiSuggestedProductDto>> FindRelevantProductsAsync(string userMessage)
    {
        var products = await _productService.GetAllAsync();
        var keywords = userMessage
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return products
            .Where(product =>
                keywords.Any(keyword =>
                    product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    || product.CategoryName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    || product.ShortDescription.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    || product.Brand.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    || product.Sport.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            .Take(5)
            .Select(product => new AiSuggestedProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryName = product.CategoryName,
                ShortDescription = product.ShortDescription
            })
            .ToList();
    }
}
