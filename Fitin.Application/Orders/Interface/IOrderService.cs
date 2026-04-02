

using Fitin.Application.Orders.DTOs;

namespace Fitin.Application.Orders.Interface;

public interface IOrderService
{
    Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId,CreateOrderDto dto);
    Task<IEnumerable<OrderDto>> GetUserOrderAsync (Guid userId);
    Task<OrderDto?> GetOrderByIdAsync(Guid userId,Guid orderId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task UpdateOrderStatusAsync(Guid orderId, string status);
}