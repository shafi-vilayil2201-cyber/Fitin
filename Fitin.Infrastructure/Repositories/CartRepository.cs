using Fitin.Application.Cart.Interfaces;
using Fitin.Infrastructure.Persistence;
using Fitin.Domain.Entities.CartItem;
using Microsoft.EntityFrameworkCore;


namespace Fitin.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync (CartItem item)
    {
        await _context.CartItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}