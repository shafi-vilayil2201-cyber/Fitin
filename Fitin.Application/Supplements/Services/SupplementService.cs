using Fitin.Domain.Entities.Supplements;
using Fitin.Application.Supplements.Interfaces;
using Fitin.Application.Supplements.DTOs;
using Fitin.Application.Common.Exceptions;
using Fitin.Application.Categories.Interface;

namespace Fitin.Application.Supplements.Services;

public class SupplementService : ISupplementService
{
    private readonly ISupplementRepository _repository;
    private readonly ICategoryRepository _categoryRepo;

    public SupplementService(
        ISupplementRepository repository,
        ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepo = categoryRepository;
    }

    public async Task<IEnumerable<SupplementDto>> GetAllAsync()
    {
        var supplements = await _repository.GetAllAsync();
        return supplements.Select(MapSupplement);
    }

    public async Task<SupplementDto?> GetByIdAsync(Guid id)
    {
        var supplement = await _repository.GetByIdAsync(id);

        if (supplement == null)
            return null;

        return MapSupplement(supplement);
    }

    public async Task<SupplementDto> CreateAsync(CreateSupplementDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found");

        var supplement = new Supplement(
            dto.Name,
            dto.Price,
            dto.CategoryId,
            dto.Stock,
            dto.ImageUrl,
            dto.Brand,
            dto.Description,
            dto.ShortDescription,
            dto.LongDescription,
            dto.Rating,
            dto.Discount
        );

        await _repository.AddAsync(supplement);

        var createdSupplement = await _repository.GetByIdAsync(supplement.Id);
        return MapSupplement(createdSupplement ?? supplement);
    }

    public async Task<SupplementDto?> UpdateAsync(Guid id, UpdateSupplementDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found");

        var supplement = await _repository.GetByIdAsync(id);

        if (supplement == null)
            return null;

        supplement.UpdateDetails(
            dto.Name,
            dto.Price,
            dto.CategoryId,
            dto.Stock,
            dto.ImageUrl,
            dto.Brand,
            dto.Description,
            dto.ShortDescription,
            dto.LongDescription,
            dto.Rating,
            dto.Discount);

        await _repository.UpdateAsync(supplement);

        var updatedSupplement = await _repository.GetByIdAsync(id);
        return MapSupplement(updatedSupplement ?? supplement);
    }

    public async Task DeleteAsync(Guid id)
    {
        var supplement = await _repository.GetByIdAsync(id);

        if (supplement == null)
            return;

        await _repository.DeleteAsync(supplement);
    }

    public async Task<IEnumerable<SupplementDto>> GetSupplementsAsync(SupplementQueryDto query)
    {
        var supplements = await _repository.GetSupplementsAsync(query);
        return supplements.Select(MapSupplement);
    }

    private static SupplementDto MapSupplement(Supplement supplement)
    {
        return new SupplementDto
        {
            Id = supplement.Id,
            Name = supplement.Name,
            Price = supplement.Price,
            CategoryId = supplement.CategoryId,
            CategoryName = supplement.Category?.Name ?? string.Empty,
            Stock = supplement.Stock,
            ImageUrl = supplement.ImageUrl,
            Brand = supplement.Brand,
            Description = supplement.Description,
            ShortDescription = supplement.ShortDescription,
            LongDescription = supplement.LongDescription,
            Rating = supplement.Rating,
            Discount = supplement.Discount
        };
    }
}
