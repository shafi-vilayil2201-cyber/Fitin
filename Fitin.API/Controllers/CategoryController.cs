

using CloudinaryDotNet.Actions;
using Fitin.Application.Categories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;



[ApiController]
[Route("api/categories")]
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Success(categories,"Category retrieved successfully");
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id )
    {
        var category =await _service.GetByIdAsync(id);

        if(category == null)
        {
            return Failure("Category not found",null,404);
        }
        return Success(category,"Category retirved successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CreateCategoryDto dto)
    {
        var category = await _service.CreateAsync(dto);
        return Success(category,"Category Added Succefully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id , CreateCategoryDto dto)
    {
        var category= await _service.GetByIdAsync(id);

        if(category == null)
            return Failure("Category not Found",null,404);
            
        return Success(category,"Category updated successfully");
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _service.GetByIdAsync(id);
        
        if (category == null)
            return Failure("Category not found", null,404);

        await _service.DeleteAsync(id);
        return Success<object?>(null,"Category deleted successfully");
    }

}
