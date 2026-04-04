

using AutoMapper;
using Fitin.Application.Orders.DTOs;
using Fitin.Domain.Entities;

namespace Fitin.Application.Common.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(
                dest => dest.TotalPrice,
                opt => opt.MapFrom(src => src.UnitPrice * src.Quantity)
            );
        
        CreateMap<Order, OrderDto>()
            .ForMember(
                dest => dest.OrderId,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.ShippingName)
            )
            .ForMember(
                dest => dest.UserEmail,
                opt => opt.MapFrom(src => src.ShippingPhone)
            )
            .ForMember(
                dest => dest.OrderDate,
                opt => opt.MapFrom(src => src.CreatedAt)
            )
            .ForMember(dest => dest.ShippingName, opt => opt.MapFrom(src => src.ShippingName))
            .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
            .ForMember(dest => dest.ShippingCity, opt => opt.MapFrom(src => src.ShippingCity))
            .ForMember(dest => dest.ShippingPostalCode, opt => opt.MapFrom(src => src.ShippingPostalCode))
            .ForMember(dest => dest.ShippingPhone, opt => opt.MapFrom(src => src.ShippingPhone))
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems)
            );
    }
}