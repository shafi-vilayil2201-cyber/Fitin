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
    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;

        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out userId);
    }

    
    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToWishlist(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        await _wishlistService.AddToWishListAsync(userId, productId);

        return Success<object?>(null, "Product added to wishlist");
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromWishist(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        await _wishlistService.RemoveFromWishlistAsync(userId, productId);

        return Success<object?>(null, "Product removed from wishlist");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserWishlist()
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var wishlist = await _wishlistService.GetUserWishListAsync(userId);

        return Success(wishlist, "Wishlist fetched successfully");
    }
    
}
