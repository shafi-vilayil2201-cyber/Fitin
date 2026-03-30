

using AutoMapper;
using Fitin.Application.Categories.DTOs;
using Fitin.Domain.Entities.Categories;

namespace Fitin.Application.Common.Mappings;

public class CategoryProfile :Profile
{
    public CategoryProfile()
    {
        CreateMap<Category , CategoryDto>();
    }


}