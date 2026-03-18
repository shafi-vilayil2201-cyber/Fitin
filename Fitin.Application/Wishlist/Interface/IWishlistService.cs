

using Fitin.Application.Wishlist.Dto;
using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Application.Wishlist.Interfaces;

public interface IWishlistService
{
    Task AddToWishListAsync(Guid userId,Guid productId);
    Task RemoveFromWishlistAsync(Guid userId,Guid productId);
    Task<IEnumerable<WishlistItemDto>> GetUserWishListAsync(Guid userId);
} 

  
