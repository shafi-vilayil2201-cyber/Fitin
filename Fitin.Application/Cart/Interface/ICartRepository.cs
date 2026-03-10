using Fitin.Domain.Entities.CartItems;

namespace Fitin.Application.Cart.Interfaces;

public interface ICartRepository
{
    Task AddToCartAsync(Guid userId,Guid ProductId);
    Task IncreaseQuantityAsync(Guid userId,Guid ProductId);
    Task DecreaseQuantityAsync(Guid userId,Guid ProductId);
    Task RemoveFromCartAsync (Guid userId,Guid ProductId);
}