using Fitin.Application.Wishlist.Interfaces;
using Fitin.Domain.Entities.Wishlists;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;




namespace Fitin.Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly AppDbContext _context;

    public WishlistRepository (AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WishlistItem item)
    {
        //To avoid adding the same product multiple times to wishlist.
        var exists = await _context.WishlistItems
        .AnyAsync(x => x.UserId == item.UserId && x.ProductId == item.ProductId);

        if (exists)
            return;
        
        await _context.WishlistItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}