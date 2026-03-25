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
    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToCart(Guid productId)
    {
        var cart = await _cartService.AddToCartAsync(GetUserId(), productId);

        return Success(cart, "Product added to cart");
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(Guid productId)
    {
        await _cartService.RemoveFromCartAsync(GetUserId(), productId);

        return Success<object?>(null, "Product removed from cart");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserCart()
    {
        var cart = await _cartService.GetUserCartAsync(GetUserId());

        return Success<object>(cart, "Cart fetched successfully");
    }

    [HttpPatch("increase/{productId}")]
    public async Task<IActionResult> IncreaseQuantity(Guid productId)
    {

        var cart = await _cartService.IncreaseQuantityAsync(GetUserId(), productId);

        return Success(cart, "Quantity increased");
    }
    [HttpPatch("decrease/{productId}")]
    public async Task<IActionResult> DecreaseQuantity(Guid productId)
    {

        await _cartService.DecreaseQuantityAsync(GetUserId(), productId);

        return Success<object?>(null, "Quantity decreased");
    }

}