using Fitin.Application.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Products.Interfaces;
using Fitin.Domain.Entities.Product;
using Microsoft.AspNetCore.Authorization;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productrespository;

    public ProductController(IProductRepository  productrespository)
    {
        _productrespository = productrespository;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllProduct()
    {
        var products =await _productrespository.GetAllAsync();

        var result = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Category = p.Category,
            Stock = p.Stock
        });
        return Ok(result);
    }

  
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetCategory(string category)
    {
        var products = await _productrespository.GetByCategoryAsync(category);

        var result = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Category= p.Category,
            Stock = p.Stock
        });

        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await _productrespository.GetByIdAsync(id);
        
        if(product == null)
            return NotFound();
        
        var result = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            Stock = product.Stock
        };

        return Ok(result);
    }

//     [Authorize(Roles = "Admin")]
//     [HttpPost]
//     public async Task<IActionResult> CreateProduct(ProductDto dto)
//     {
//         var product = new Product(dto.Name, dto.Price, dto.Category, dto.Stock ,dto.ImageUrl);

//         await _productrespository.AddAsync(product);

//         var result = new ProductDto
//         {
//             Id = product.Id,
//             Name = product.Name,
//             Price = product.Price,
//             Category = product.Category,
//             Stock = product.Stock,
//             ImageUrl = product.ImageUrl
//         };

//         return CreatedAtAction(nameof(GetByIdAsync), new { id = product.Id }, result);
//     }
}
