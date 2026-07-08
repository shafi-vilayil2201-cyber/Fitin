using Fitin.Application.Common.Interfaces;
using Fitin.Application.Supplements.DTOs;
using Fitin.Domain.Entities.Supplements;

namespace Fitin.Application.Supplements.Interfaces;

public interface ISupplementRepository : IGenericRepository<Supplement>
{
    new Task<Supplement?> GetByIdAsync(Guid id);
    new Task<IEnumerable<Supplement>> GetAllAsync();
    Task<IEnumerable<Supplement>> GetSupplementsAsync(SupplementQueryDto query);
}
