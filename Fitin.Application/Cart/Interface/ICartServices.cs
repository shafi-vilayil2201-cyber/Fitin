
using Fitin.Application.Cart.Dto;

namespace Fitin.Application.Cart.Interfaces;

public interface ICartService 
{
    Task AddToCartAsync(Guid userId,Guid productId);
    Task RemoveFromCartAsync(Guid userId,Guid productId);
    Task IncreaseQuantityAsync(Guid userId, Guid productId);

    Task DecreaseQuantityAsync(Guid userId, Guid productId);
    Task<IEnumerable<CartItemDto>> GetUserCartAsync(Guid userId);
}
