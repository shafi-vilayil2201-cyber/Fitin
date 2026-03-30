

using Fitin.Application.Categories.DTOs;

namespace Fitin.Application.Categories.Interface;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateAsync(Guid id,UpdateCategoryDto dto);
    Task DeleteAsync(Guid id);
}
