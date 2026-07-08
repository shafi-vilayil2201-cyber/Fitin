using Fitin.Application.Supplements.DTOs;

namespace Fitin.Application.Supplements.Interfaces;

public interface ISupplementService
{
    Task<IEnumerable<SupplementDto>> GetAllAsync();
    Task<SupplementDto?> GetByIdAsync(Guid id);
    Task<SupplementDto> CreateAsync(CreateSupplementDto dto);
    Task<SupplementDto?> UpdateAsync(Guid id, UpdateSupplementDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<SupplementDto>> GetSupplementsAsync(SupplementQueryDto query);
}
