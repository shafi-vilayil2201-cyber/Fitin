using AutoMapper;
using Fitin.Application.Cart.Dto;
using Fitin.Application.Cart.Interfaces;
using Fitin.Application.Products.Interfaces;
using Fitin.Domain.Entities.CartItems;

namespace Fitin.Application.Cart.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    private const int MAX_CART_QUANTITY = 10;

    public CartService(
        ICartRepository cartRepository,
        IMapper mapper,
        IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _productRepository = productRepository;
    }
    
    public async Task<AddToCartResultDto> AddToCartAsync(Guid userId, Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new Exception("Product not found");

        if (product.Stock <= 0)
            throw new Exception("Product out of stock");

        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

        if (cartItem != null)
        {
            var existingCartItems = await _cartRepository.GetUserCartAsync(userId);
            var existingItem = existingCartItems.FirstOrDefault(x => x.ProductId == productId);

            return new AddToCartResultDto
            {
                Message = "Item already in cart",
                Item = existingItem == null ? null : _mapper.Map<CartItemDto>(existingItem)
            };
        }

        var newItem = new CartItem(userId, productId);
        await _cartRepository.AddAsync(newItem);
        await _cartRepository.SaveChangesAsync();

        var cartItems = await _cartRepository.GetUserCartAsync(userId);
        var addedItem = cartItems.FirstOrDefault(x => x.ProductId == productId);

        return new AddToCartResultDto
        {
            Message = "Product added to cart",
            Item = addedItem == null ? null : _mapper.Map<CartItemDto>(addedItem)
        };
    }


    public async Task RemoveFromCartAsync(Guid userId, Guid productId)
    {
        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

        if (cartItem == null)
            return;

        await _cartRepository.RemoveAsync(cartItem);

        await _cartRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CartItemDto>> GetUserCartAsync(Guid userId)
    {
        var cartItems = await _cartRepository.GetUserCartAsync(userId);

        return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }

    public async Task<IEnumerable<CartItemDto>> IncreaseQuantityAsync(Guid userId, Guid productId)
    {
        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

        if (cartItem == null)
            throw new Exception("Cart item not found");

        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            throw new Exception("Product not found");

        if (cartItem.Quantity >= MAX_CART_QUANTITY)
            throw new Exception("Maximum cart quantity reached");

        if (cartItem.Quantity + 1 > product.Stock)
            throw new Exception("Not enough stock available");

        cartItem.IncreaseQuantity();

        await _cartRepository.SaveChangesAsync();

        var cartItems = await _cartRepository.GetUserCartAsync(userId);
        return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }

    public async Task<IEnumerable<CartItemDto>> DecreaseQuantityAsync(Guid userId, Guid productId)
    {
        var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

        if (cartItem == null)
            throw new Exception("Cart item not found");

        cartItem.DecreaseQuantity();

        if (cartItem.Quantity <= 0)
        {
            await _cartRepository.RemoveAsync(cartItem);
        }

        await _cartRepository.SaveChangesAsync();

        var cartItems = await _cartRepository.GetUserCartAsync(userId);
        return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }
}
