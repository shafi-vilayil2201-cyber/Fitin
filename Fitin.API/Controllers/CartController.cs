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
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    private Guid GetUserId ()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = GetUserId();

        await _cartRepository.AddToCartAsync(userId,dto.ProductId);

        return Ok("Product added to cart");
    }
    [HttpPatch("increase")]
    public async Task<IActionResult> IncreaseQuantity(AddToCartDto dto)
    {
        var userId = GetUserId();

        await _cartRepository.IncreaseQuantityAsync(userId,dto.ProductId);

        return Ok("Quantity increased");
    }
    [HttpPatch("decrease")]
    public async Task<IActionResult> DecreaseQuantity(AddToCartDto dto)
    {
        var userId = GetUserId();

        await _cartRepository.DecreaseQuantityAsync(userId,dto.ProductId);

        return Ok("Quantity decreased");
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(Guid productId)
    {
        var userId = GetUserId();

        await _cartRepository.RemoveFromCartAsync(userId,productId);

        return Ok("Product removed from cart");
    }

}