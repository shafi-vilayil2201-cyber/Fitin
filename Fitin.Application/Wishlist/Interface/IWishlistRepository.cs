using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Application.Wishlist.Interfaces;

public interface IWishlistRepository
{
    Task AddAsync(WishlistItem item);

}
