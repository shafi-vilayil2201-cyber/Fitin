using Fitin.Application.Common.Interfaces;
using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Application.Wishlist.Interfaces;

public interface IWishlistRepository :IGenericRepository<WishlistItem>
{

    Task<WishlistItem?> GetWishlistItemAsync(Guid userId, Guid productId);

    Task<List<WishlistItem>> GetUserWishlistAsync(Guid userId);
}