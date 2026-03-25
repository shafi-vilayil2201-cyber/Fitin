using Fitin.Domain.Entities.CartItems;

namespace Fitin.Application.Cart.Interfaces;


public interface ICartRepository
{
    Task<CartItem?> GetCartItemAsync(Guid userId, Guid productId);

    Task<IEnumerable<CartItem>> GetUserCartAsync(Guid userId);

    Task AddAsync(CartItem item);

    Task RemoveAsync(CartItem item);

    Task SaveChangesAsync();
}
