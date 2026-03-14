

using AutoMapper;
using Fitin.Application.Products.Dto;
using Fitin.Domain.Entities.Products;

namespace Fitin.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto ,Product>();
    }
}
