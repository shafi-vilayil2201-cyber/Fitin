


using Fitin.Domain.Entities;

namespace Fitin.Application.Orders.Interface;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid oderId);
    Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId);
    Task SaveChangesAsync();
}