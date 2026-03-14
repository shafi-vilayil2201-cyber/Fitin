using Fitin.Application.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Products.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Fitin.Domain.Entities.Products;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;

    public AdminProductsController(IProductRepository productRepository, IImageService imageService)
    {
        _productRepository = productRepository;
        _imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product(dto.Name, dto.Price, dto.Category, dto.Stock, dto.ImageUrl);
        await _productRepository.AddAsync(product);

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            Stock = product.Stock
        });
    } 
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var url = await _imageService.UploadImageAsync(stream, file.FileName);

        return Ok(url);
    }
}
