


using AutoMapper;
using Fitin.Application.Categories.Interface;
using Fitin.Domain.Entities.Categories;

namespace Fitin.Application.Categories.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IMapper _mapper;

    public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper)
    {
        _categoryRepo = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        
        if(category == null)
            return null;
        
        return _mapper.Map<CategoryDto>(category);
    }
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var existing = await _categoryRepo.GetByNameAsync(dto.Name);

        if (existing != null)
             throw new Exception("Category already existing");

        var category = new Category(dto.Name);
        await _categoryRepo.AddAsync(category);


        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id,UpdateCategoryDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        
        if(category == null)
            return null;
        
        category.UpdateName(dto.Name);
        await _categoryRepo.UpdateAsync(category);

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if(category == null)
            return;

        await _categoryRepo.DeleteAsync(category);
    }

}
 
