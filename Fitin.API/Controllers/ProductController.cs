using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Products.Dto;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _service.GetByCategoryAsync(category);
        return Ok(products);
    }
    [HttpGet("Search")]
    public async Task<IActionResult> GetProduct([FromQuery]ProductQueryDto query)
    {
        var products =await _service.GetProductsAsync(query);
        return Ok(products);
    }

}