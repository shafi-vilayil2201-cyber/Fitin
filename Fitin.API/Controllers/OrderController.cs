


using System.Security.Claims;
using Fitin.Application.Orders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitin.Application.Orders.DTOs;

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

    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;

        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out userId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var result = await _orderService.CreateOrderAsync(userId, dto);

        return CreatedResponse(result,"Order created successfully");
    }
    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var orders = await _orderService.GetUserOrderAsync(userId);

        return Success(orders,"Orders fetched successfully");
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Failure("Invalid or missing auth token", statusCode: 401);

        var order = await _orderService.GetOrderByIdAsync(userId, id);

        if(order == null)
            return Failure("order not found", statusCode: 404);
        
        return Success(order,"Order fetched successfully");
        
    }
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Success(orders, "All orders fetched");
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
    {
        await _orderService.UpdateOrderStatusAsync(id, dto.Status);
        return Success("Order status updated");
    }

    [HttpPost("confirm-payment")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
    {
        var result = await _orderService.ConfirmPaymentAsync(dto);
        if (result)
            return Success(true, "Payment verified successfully");
        else
            return Failure("Payment verification failed", statusCode: 400);
    }

}
