


using AutoMapper;
using Fitin.Application.Users.DTOs;
using Fitin.Domain.Entities;

namespace Fitin.Application.Common.Mappings;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User , UserListItemDto>();
        CreateMap<User , UserDetailsDto>()
            .ForMember(dest => dest.CartItemCount, opt => opt.MapFrom(src => src.CartItems != null ? src.CartItems.Count : 0));
    }
}
    
