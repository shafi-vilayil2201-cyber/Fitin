using Fitin.Domain.Entities.CartItems;

namespace Fitin.Application.Cart.Interfaces;

public interface ICartRepository
{
    Task AddAsync(CartItem item);

}