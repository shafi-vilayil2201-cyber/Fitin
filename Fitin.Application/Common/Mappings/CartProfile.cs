

using AutoMapper;
using Fitin.Application.Cart.Dto;
using Fitin.Domain.Entities.CartItems;

namespace Fitin.Application.Common.Mappings;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartItem, CartItemDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            )
            .ForMember(
                dest => dest.ProductPrice,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0m)
            )
            .ForMember(
                dest => dest.ProductImageUrl,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty)
            );
    }
}
