using Fitin.Domain.Common;

namespace Fitin.Domain.Entities.Category;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public Category(string name)
    {
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
        MarkUpdated();
    }
}
