

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
                dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems)
            );
    }
}