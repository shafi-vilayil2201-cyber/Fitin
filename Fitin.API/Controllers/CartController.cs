using Fitin.Application.Cart.Interfaces;
using Fitin.Domain.Entities.CartItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fitin.Application.Cart.Dto;
using System.Security.Claims;

namespace Fitin.API.Controllers;


[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : BaseApiController
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;

        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out userId);
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToCart(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var result = await _cartService.AddToCartAsync(userId, productId);

        return Success(result.Item, result.Message);
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        await _cartService.RemoveFromCartAsync(userId, productId);

        return Success<object?>(null, "Product removed from cart");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserCart()
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var cart = await _cartService.GetUserCartAsync(userId);

        return Success<object>(cart, "Cart fetched successfully");
    }

    [HttpPatch("increase/{productId}")]
    public async Task<IActionResult> IncreaseQuantity(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var cart = await _cartService.IncreaseQuantityAsync(userId, productId);

        return Success(cart, "Quantity increased");
    }
    [HttpPatch("decrease/{productId}")]
    public async Task<IActionResult> DecreaseQuantity(Guid productId)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        await _cartService.DecreaseQuantityAsync(userId, productId);

        return Success<object?>(null, "Quantity decreased");
    }
}
