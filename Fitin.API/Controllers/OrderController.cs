


using System.Security.Claims;
using Fitin.Application.Orders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;

    public OrderController (IOrderService orderService)
    {
        _orderService = orderService;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
        var result = await _orderService.CreateOrderAsync(GetUserId());

        return CreatedResponse(result,"Order created successfully");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var orders = await _orderService.GetUserOrderAsync(GetUserId());

        return Success(orders,"Orders fetched successfully");
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(GetUserId(),id);

        if(order == null)
            return Failure("order not found", statusCode: 404);
        
        return Success(order,"Order fetched successfully");
    }
}