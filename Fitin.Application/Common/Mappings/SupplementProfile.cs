using AutoMapper;
using Fitin.Application.Supplements.DTOs;
using Fitin.Domain.Entities.Supplements;

namespace Fitin.Application.Common.Mappings;

public class SupplementProfile : Profile
{
    public SupplementProfile()
    {
        CreateMap<Supplement, SupplementDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
    }
}
