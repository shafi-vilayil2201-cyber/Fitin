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
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    private Guid GetUserId ()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToCart(Guid productId)
    {
        await _cartService.AddToCartAsync(GetUserId(),productId);

        return Ok("Product added to cart");
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(Guid productId)
    {
        await _cartService.RemoveFromCartAsync(GetUserId(),productId);

        return Ok("Product removed from cart");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserCart()
    {
        var cart = await _cartService.GetUserCartAsync(GetUserId());

        return Ok(cart);
    }

    [HttpPatch("increase/{productId}")]
    public async Task<IActionResult> IncreaseQuantity(Guid productId)
    {

        await _cartService.IncreaseQuantityAsync(GetUserId(),productId);

        return Ok("Quantity increased");
    }
    [HttpPatch("decrease/{productId}")]
    public async Task<IActionResult> DecreaseQuantity(Guid productId)
    {

        await _cartService.DecreaseQuantityAsync(GetUserId(),productId);

        return Ok("Quantity decreased");
    }
    
}