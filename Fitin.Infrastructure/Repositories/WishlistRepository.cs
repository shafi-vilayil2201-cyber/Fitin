using Fitin.Application.Wishlist.Interfaces;
using Fitin.Domain.Entities.Wishlists;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;




namespace Fitin.Infrastructure.Repositories;

public class WishlistRepository : GenericRepository<WishlistItem>, IWishlistRepository
{
    public WishlistRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<WishlistItem?> GetWishlistItemAsync(Guid userId, Guid productId)
    {
        return await _context.WishlistItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
    }
    public async Task<List<WishlistItem> >GetUserWishlistAsync(Guid userId)
    {

        return  await _context.WishlistItems
            .Where(x=> x.UserId == userId)
            .Include(x => x.Product)
            .ToListAsync();
    }

}