using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Products.Dto;
namespace Fitin.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : BaseApiController
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
        return Success(products,"Products retrieved successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return Failure("Product not found",null,404);

        return Success(product,"Product retrieved successfully");
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _service.GetByCategoryAsync(category);
        return Success(products,"Products retrieved by category");
    }
    [HttpGet("search")]
    public async Task<IActionResult> GetProduct([FromQuery]ProductQueryDto query)
    {
        var products =await _service.GetProductsAsync(query);
        return Success(products,"Products retrieved by Search");
    }

}