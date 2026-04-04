

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

    public async Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId,CreateOrderDto dto)
    {
        var cartItems = (await _cartRepository.GetUserCartAsync(userId)).ToList();

        if (!cartItems.Any())
            throw new BadRequestException("Cart is empty");

        decimal totalAmount = 0m;
        var cartData = new List<(CartItem CartItem,Product Product)>();

        foreach (var cartItem in cartItems)
        {
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

            if (product == null)
                throw new NotFoundException($"Product not found: {cartItem.ProductId}");

            if(product.Stock < cartItem.Quantity)
                throw new BadRequestException($"not enough stock: {product.Name} ");

            totalAmount += product.Price * cartItem.Quantity;
            cartData.Add((cartItem,product));
        }

        var order = new Order(userId, totalAmount, dto.ShippingName, dto.ShippingAddress, dto.ShippingCity, dto.ShippingPostalCode, dto.ShippingPhone);

        foreach (var item in cartData)
        {
            
            var orderItem = new OrderItem(
                order.Id,
                item.Product.Id,
                item.Product.Name,
                item.Product.ImageUrl,
                item.Product.Price,
                item.CartItem.Quantity

            );

            order.AddOrderItem(orderItem);
            item.Product.ReduceStock(item.CartItem.Quantity);
            await _cartRepository.RemoveAsync(item.CartItem);
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