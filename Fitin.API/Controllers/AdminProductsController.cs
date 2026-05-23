using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Products.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Fitin.Application.Products.Dto;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductsController : BaseApiController
{
    private readonly IProductService _productService;
    private readonly IServiceProvider _serviceProvider;

    public AdminProductsController(IProductService productService, IServiceProvider serviceProvider)
    {
        _productService = productService;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Success(products, "Products retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return Failure("Product not found", null, 404);

        return Success(product, "Product retrieved successfully");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductDto dto, IFormFile? image)
    {
        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var product = await _productService.CreateAsync(dto);
        return CreatedResponse(product, "Product created successfully");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateProductDto dto, IFormFile? image)
    {
        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var product = await _productService.UpdateAsync(id, dto);

        if (product == null)
            return Failure("Product not found", null, 404);

        return Success(product, "Product updated successfully");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return Failure("Product not found", null, 404);

        await _productService.DeleteAsync(id);
        return Success<object?>(null, "Product deleted successfully");
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Failure("Image file is required");

        using var stream = file.OpenReadStream();

        var imageService = _serviceProvider.GetRequiredService<IImageService>();
        var url = await imageService.UploadImageAsync(stream, file.FileName);

        return Success(url, "Image uploaded successfully");
    }
}
