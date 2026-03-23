using System.Security.Claims;
using Fitin.Application.Wishlist.Dto;
using Fitin.Application.Wishlist.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/wishlist")]
[Authorize]
public class WishlistController : BaseApiController
{
    private readonly IWishlistService _wishlistService;
    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }
    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    
    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToWishlist(Guid productId)
    {

        await _wishlistService.AddToWishListAsync(GetUserId(),productId);

        return Success<object?>(null, "Product added to wishlist");
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromWishist(Guid productId)
    {
        await _wishlistService.RemoveFromWishlistAsync(GetUserId(),productId);

        return Success<object?>(null, "Product removed from wishlist");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserWishlist()
    {
        var wishlist = await _wishlistService.GetUserWishListAsync(GetUserId());

        return Success(wishlist, "Wishlist fetched successfully");
    }
    
}
