

using AutoMapper;
using Fitin.Application.Cart.Interfaces;
using Fitin.Application.Common.Exceptions;
using Fitin.Application.Common.Interfaces;
using Fitin.Application.Orders.DTOs;
using Fitin.Application.Orders.Interface;
using Fitin.Application.Products.Interfaces;
using Fitin.Domain.Entities;
using Fitin.Domain.Entities.CartItems;
using Fitin.Domain.Entities.Products;

namespace Fitin.Application.Orders.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService (
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper= mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        var itemsToOrder = new List<(Guid ProductId, int Quantity)>();

        if (dto.ProductId.HasValue)
        {
            // Direct purchase mode
            itemsToOrder.Add((dto.ProductId.Value, dto.Quantity));
        }
        else
        {
            // Cart-based mode
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
                throw new BadRequestException("Cart is empty");

            itemsToOrder.AddRange(cartItems.Select(c => (c.ProductId, c.Quantity)));
        }

        decimal totalAmount = 0m;
        var orderData = new List<(Product Product, int Quantity)>();

        foreach (var item in itemsToOrder)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product == null)
                throw new NotFoundException($"Product not found: {item.ProductId}");

            if (product.Stock < item.Quantity)
                throw new BadRequestException($"Not enough stock: {product.Name}");

            totalAmount += product.Price * item.Quantity;
            orderData.Add((product, item.Quantity));
        }

        var order = new Order(userId, totalAmount, dto.ShippingName, dto.ShippingAddress, dto.ShippingCity, dto.ShippingPostalCode, dto.ShippingPhone);

        foreach (var data in orderData)
        {
            var orderItem = new OrderItem(
                order.Id,
                data.Product.Id,
                data.Product.Name,
                data.Product.ImageUrl,
                data.Product.Price,
                data.Quantity
            );

            order.AddOrderItem(orderItem);
            data.Product.ReduceStock(data.Quantity);

            // If it was a cart order, remove from cart
            if (!dto.ProductId.HasValue)
            {
                var cartItem = (await _cartRepository.GetUserCartAsync(userId))
                    .FirstOrDefault(c => c.ProductId == data.Product.Id);
                if (cartItem != null)
                    await _cartRepository.RemoveAsync(cartItem);
            }
        }
        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return new CreateOrderResponseDto
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status
        };

    }
    public async Task<IEnumerable<OrderDto>> GetUserOrderAsync(Guid userId)
    {
        var orders = await _orderRepository.GetUserOrdersAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid userId,Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if(order == null || order.UserId != userId) 
            return null;
        
        return _mapper.Map<OrderDto>(order);
    }
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
    public async Task UpdateOrderStatusAsync(Guid orderId ,string status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if(order== null) throw new NotFoundException("Order not Found");
        order.UpdateStatus(status);
        await _unitOfWork.SaveChangesAsync();
    }
}