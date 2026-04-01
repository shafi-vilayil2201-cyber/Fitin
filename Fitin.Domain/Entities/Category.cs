using Fitin.Domain.Common;

namespace Fitin.Domain.Entities.Categories;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? ImageUrl {get;set;}

    private Category() { }

    public Category(string name,string? imageUrl =null)
    {
        Name = name;
        ImageUrl = imageUrl;
    }

    public void UpdateName(string name,string? imageUrl)
    {
        Name = name;
        ImageUrl = imageUrl;
        MarkUpdated();
    }
}
