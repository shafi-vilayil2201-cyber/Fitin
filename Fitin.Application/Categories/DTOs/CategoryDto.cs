namespace Fitin.Application.Categories.DTOs;

public class CategoryDto
{
    public Guid Id{get;set;}
    public string Name{get; set;}= string.Empty;
    public string? ImageUrl{get;set;}
    public DateTime CreatedAt { get; set; }
}

public class CreateCategoryDto
{
    public string Name{get;set;}=string.Empty;
    public string? ImageUrl{get;set;}
}
public class UpdateCategoryDto
{
    public string Name{get;set;}=string.Empty;
    public string? ImageUrl{get;set;}
}
