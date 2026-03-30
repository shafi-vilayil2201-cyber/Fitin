using Fitin.Application.Cart.Interfaces;
using Fitin.Infrastructure.Persistence;
using Fitin.Domain.Entities.CartItems;
using Microsoft.EntityFrameworkCore;



namespace Fitin.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository (AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartItem?> GetCartItemAsync(Guid userId ,Guid productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
    }
    public async Task<IEnumerable<CartItem>> GetUserCartAsync(Guid userId)
    {
        return await _context.CartItems
            .Where(x => x.UserId == userId)
            .Include(x => x.Product)
            .ToListAsync();
    }

    public async Task AddAsync (CartItem item)
    {
         await _context.CartItems.AddAsync(item);

    }
    public async Task RemoveAsync(CartItem item)
    {
         _context.CartItems.Remove(item);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
