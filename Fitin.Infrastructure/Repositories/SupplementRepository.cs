using Fitin.Application.Supplements.Interfaces;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Fitin.Domain.Entities.Supplements;
using Fitin.Application.Supplements.DTOs;

namespace Fitin.Infrastructure.Repositories;

public class SupplementRepository : GenericRepository<Supplement>, ISupplementRepository
{
    public SupplementRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Supplement?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public new async Task<IEnumerable<Supplement>> GetAllAsync()
    {
        return await _dbSet
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Supplement>> GetSupplementsAsync(SupplementQueryDto query)
    {
        var supplements = _context.Set<Supplement>()
            .Include(p => p.Category)
            .AsQueryable();

        if (query.CategoryId.HasValue)
        {
            supplements = supplements.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            switch (query.Sort.ToLower())
            {
                case "price_asc":
                    supplements = supplements.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    supplements = supplements.OrderByDescending(p => p.Price);
                    break;
            }
        }
        return await supplements.ToListAsync();
    }
}
