using AutoMapper;
using Fitin.Application.Wishlist.Dto;
using Fitin.Domain.Entities.Wishlists;

namespace Fitin.Application.Common.Mappings;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<WishlistItem, WishlistItemDto>()
            .ForMember(
                dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId)
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)
            )
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0m)
            )
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty)
            );
    }
}
