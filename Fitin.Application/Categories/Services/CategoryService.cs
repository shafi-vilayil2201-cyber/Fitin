using Fitin.Application.Categories.DTOs;
using Fitin.Application.Categories.Interface;
using Fitin.Application.Common.Exceptions;
using Fitin.Domain.Entities.Categories;

namespace Fitin.Application.Categories.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(
            ICategoryRepository categoryRepository)
    {
        _categoryRepo = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return categories.Select(MapCategory);
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        
        if(category == null)
            return null;
        
        return MapCategory(category);
    }
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var existing = await _categoryRepo.GetByNameAsync(dto.Name);

        if (existing != null)
             throw new BadRequestException("Category already exists");

        var category = new Category(dto.Name, dto.ImageUrl);
        await _categoryRepo.AddAsync(category);

        return MapCategory(category);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id,UpdateCategoryDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        
        if(category == null)
           throw new NotFoundException("Category not found");

        var existing = await _categoryRepo.GetByNameAsync(dto.Name);

        if (existing != null && existing.Id != id)
            throw new BadRequestException("Category already exists");
        
        category.UpdateName(dto.Name, dto.ImageUrl);
        await _categoryRepo.UpdateAsync(category);

        return MapCategory(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if(category == null)
            return;

        await _categoryRepo.DeleteAsync(category);
    }

    private static CategoryDto MapCategory(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            CreatedAt = category.CreatedAt
        };
    }

}
