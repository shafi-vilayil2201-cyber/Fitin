using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Supplements.Interfaces;
using Fitin.Application.Supplements.DTOs;
using Microsoft.AspNetCore.Authorization;
using Fitin.Application.Categories.Interface;
using Fitin.Application.Products.Interfaces;
using Fitin.Application.Common.Interfaces;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/admin/supplements")]
[Authorize(Roles = "Admin")]
public class AdminSupplementsController : BaseApiController
{
    private readonly ISupplementService _supplementService;
    private readonly ICategoryService _categoryService;
    private readonly IServiceProvider _serviceProvider;
    private static readonly HashSet<string> SupplementCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "Protein", "Energy", "Wellness", "Hydration", "Recovery"
    };

    public AdminSupplementsController(
        ISupplementService supplementService,
        ICategoryService categoryService,
        IServiceProvider serviceProvider)
    {
        _supplementService = supplementService;
        _categoryService = categoryService;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var supplements = await _supplementService.GetAllAsync();
        return Success(supplements, "Supplement products retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplement = await _supplementService.GetByIdAsync(id);

        if (supplement == null)
            return Failure("Supplement product not found", null, 404);

        return Success(supplement, "Supplement product retrieved successfully");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateSupplementDto dto, IFormFile? image)
    {
        var category = await _categoryService.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Failure("Category not found", null, 404);

        if (!SupplementCategories.Contains(category.Name))
            return Failure($"Category '{category.Name}' is not a valid supplement category.", null, 400);

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var supplement = await _supplementService.CreateAsync(dto);
        return CreatedResponse(supplement, "Supplement product created successfully");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateSupplementDto dto, IFormFile? image)
    {
        var supplement = await _supplementService.GetByIdAsync(id);
        if (supplement == null)
            return Failure("Supplement product not found", null, 404);

        var category = await _categoryService.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Failure("Category not found", null, 404);

        if (!SupplementCategories.Contains(category.Name))
            return Failure($"Category '{category.Name}' is not a valid supplement category.", null, 400);

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var imageService = _serviceProvider.GetRequiredService<IImageService>();
            dto.ImageUrl = await imageService.UploadImageAsync(stream, image.FileName);
        }

        var updatedSupplement = await _supplementService.UpdateAsync(id, dto);
        return Success(updatedSupplement, "Supplement product updated successfully");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplement = await _supplementService.GetByIdAsync(id);
        if (supplement == null)
            return Failure("Supplement product not found", null, 404);

        await _supplementService.DeleteAsync(id);
        return Success<object?>(null, "Supplement product deleted successfully");
    }
}
