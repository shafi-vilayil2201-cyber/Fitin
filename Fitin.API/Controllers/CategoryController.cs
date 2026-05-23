using Fitin.Application.Categories.DTOs;
using Fitin.Application.Categories.Interface;
using Fitin.Application.Products.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _service;
    private readonly IServiceProvider _serviceProvider;

    public CategoryController(ICategoryService service, IServiceProvider serviceProvider)
    {
        _service = service;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Success(categories,"Categories retrieved successfully");
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id )
    {
        var category = await _service.GetByIdAsync(id);

        if(category == null)
        {
            return Failure("Category not found",null,404);
        }
        return Success(category,"Category retrieved successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCategoryDto dto, IFormFile? image)
    {
        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var category = await _service.CreateAsync(dto);
        return CreatedResponse(category,"Category Added Successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id ,[FromForm] UpdateCategoryDto dto, IFormFile? image)
    {
        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var category= await _service.UpdateAsync(id,dto);
            
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
        return Success<object?>(null, "Category deleted successfully");
    }

}
