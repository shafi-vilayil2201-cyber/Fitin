
using Fitin.Application.Categories.Interface;
using Fitin.Domain.Entities.Categories;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Fitin.Infrastructure.Repositories;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}



// namespace Fitin.Infrastructure.Repositories;

// public class CategoryRepository : ICategoryRepository
// {
//     private readonly AppDbContext _context;

//     public CategoryRepository(AppDbContext context)
//     {
//         _context = context;
//     }
//     public async Task<IEnumerable<Category>> GetAllAsync()
//     {
//         return await _context.Categories.ToListAsync();
//     }

//     public async Task<Category?> GetByIdAsync(Guid id)
//     {
//         return await _context.Categories.FirstOrDefaultAsync(x=> x.Id == id);
//     }
//     public async Task<Category?> GetByNameAsync(string name)
//     {
//         return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
//     }

//     public async Task AddAsync(Category category)
//     {
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();
//     }
//     public async Task UpdateAsync(Category category)
//     {
//         _context.Categories.Update(category);
//         await _context.SaveChangesAsync();
//     }
//     public async Task DeleteAsync(Category category)
//     {
//         _context.Categories.Remove(category);
//         await _context.SaveChangesAsync();
//     }
// }