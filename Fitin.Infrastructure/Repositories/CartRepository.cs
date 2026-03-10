using Fitin.Application.Cart.Interfaces;
using Fitin.Infrastructure.Persistence;
using Fitin.Domain.Entities.CartItems;
using Microsoft.EntityFrameworkCore;


namespace Fitin.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddToCartAsync(Guid userId, Guid productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity++;
        }
        else
        {
            var newItem = new CartItem(userId, productId, 1,DateTime.UtcNow);
            await _context.CartItems.AddAsync(newItem);
        }
        await _context.SaveChangesAsync();
    }
    public async Task IncreaseQuantityAsync(Guid userId,Guid productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        
        if (cartItem == null)
            throw new Exception("Cart item not Found");

        cartItem.Quantity++;

        await _context.SaveChangesAsync();


    }
    public async Task DecreaseQuantityAsync(Guid userId,Guid productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        
        if (cartItem == null)
            throw new Exception("Cart item not Found");
        
        cartItem.Quantity--;

        if(cartItem.Quantity <= 0)
        {
            _context.CartItems.Remove(cartItem);
        }

        await _context.SaveChangesAsync();
    }
    public async Task RemoveFromCartAsync(Guid userid, Guid productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.UserId == userid && x.ProductId == productId);

        if(cartItem == null)
            throw new Exception("product not found");

        _context.CartItems.Remove(cartItem);

        await _context.SaveChangesAsync();
    }
}