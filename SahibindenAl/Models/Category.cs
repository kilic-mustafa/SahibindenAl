namespace SahibindenAl.Models;

public class Category : BaseEntity
{
    public string? Name { get; set; } 
    public string? Slug { get; set; }
    
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }

    public ICollection<Advert> Adverts { get; set; }
    
    public ICollection<CategoryPropertyKey> CategoryPropertyKeys { get; set; }

    public Category()
    {
        SubCategories = new List<Category>();
        Adverts = new List<Advert>();
        CategoryPropertyKeys = new List<CategoryPropertyKey>();
    }
}