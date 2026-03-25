
using Fitin.Application.Cart.Dto;

namespace Fitin.Application.Cart.Interfaces;

public interface ICartService 
{
    Task<string> AddToCartAsync(Guid userId,Guid productId);
    Task RemoveFromCartAsync(Guid userId,Guid productId);
    Task<IEnumerable<CartItemDto>> IncreaseQuantityAsync(Guid userId, Guid productId);

    Task<IEnumerable<CartItemDto>> DecreaseQuantityAsync(Guid userId, Guid productId);
    Task<IEnumerable<CartItemDto>> GetUserCartAsync(Guid userId);
}
