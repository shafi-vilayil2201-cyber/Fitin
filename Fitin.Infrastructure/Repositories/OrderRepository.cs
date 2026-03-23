

using Fitin.Application.Orders.Interface;
using Fitin.Domain.Entities;
using Fitin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fitin.Infrastructure.Repositories;

public class OrderRepository :IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;

    }
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }
    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(x=>x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId)
    {
        return await _context.Orders
            .Where(x => x.UserId == userId)
            .Include(x => x.OrderItems)
            .ToListAsync();
    }
}