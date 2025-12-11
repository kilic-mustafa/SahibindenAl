namespace SahibindenAl.Models;

public class CategoryPropertyOption : BaseEntity
{
    public string Value { get; set; } = null!;

    public int CategoryPropertyKeyId { get; set; }
    public CategoryPropertyKey? CategoryPropertyKey { get; set; }
}
