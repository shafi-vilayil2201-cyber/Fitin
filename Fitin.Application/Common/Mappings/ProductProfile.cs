

using AutoMapper;
using Fitin.Application.Products.Dto;
using Fitin.Domain.Entities.Products;

namespace Fitin.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
                


    }
}
