using Fitin.Application.Cart.Interfaces;
using Fitin.Domain.Entities.CartItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fitin.Application.Cart.Dto;
using System.Security.Claims;

namespace Fitin.API.Controllers;


[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var CartItem = new CartItem(
            Guid.Parse(userId!),
            dto.ProductId,
            dto.Quantity,
            DateTime.UtcNow
        );

        await _cartRepository.AddAsync(CartItem);

        return Ok("Product added to cart");
    }


}