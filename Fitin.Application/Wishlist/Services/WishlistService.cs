using AutoMapper;
using Fitin.Application.Wishlist.Dto;
using Fitin.Application.Wishlist.Interfaces;
using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Application.Wishlist.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IMapper _mapper;
    public WishlistService (IWishlistRepository wishlistRepository,IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    }

    public async Task AddToWishListAsync(Guid userId,Guid productId)
    {
        var exists = await _wishlistRepository.GetWishlistItemAsync(userId,productId);

        if(exists != null)
            throw new Exception("Product Already in WishList");

        var wishlistItem = new WishlistItem(userId,productId);

        await _wishlistRepository.AddAsync(wishlistItem);
    }
    public async Task RemoveFromWishlistAsync(Guid userId,Guid productId)
    {
        var item = await _wishlistRepository.GetWishlistItemAsync(userId,productId);

        if(item == null)
            throw new Exception("Wishlist item not Found");

        await _wishlistRepository.DeleteAsync(item);
    }
    public async Task<IEnumerable<WishlistItemDto>> GetUserWishListAsync(Guid userId)
    {
        var item = await _wishlistRepository.GetUserWishlistAsync(userId);

        return _mapper.Map<IEnumerable<WishlistItemDto>>(item);
    }
}

    
