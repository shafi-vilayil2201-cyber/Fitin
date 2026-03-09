using Fitin.Domain.Entities.CartItem;

namespace Fitin.Application.Cart.Interfaces;

public interface ICartRepository
{
    Task AddAsync(CartItem item);

}