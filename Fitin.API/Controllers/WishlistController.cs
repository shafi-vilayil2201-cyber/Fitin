using System.Security.Claims;
using Fitin.Application.Wishlist.Dto;
using Fitin.Application.Wishlist.Interfaces;
using Fitin.Domain.Entities.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/wishlist")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistRepository _repository;


    public WishlistController(IWishlistRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToWishlist(AddToWishlistDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var item = new WishlistItem(
           Guid.Parse(userId!),
           dto.ProductId
        );

        await _repository.AddAsync(item);

        return Ok("product is added to wishlist");
    }


}