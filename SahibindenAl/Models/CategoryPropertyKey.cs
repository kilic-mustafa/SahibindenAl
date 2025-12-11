namespace SahibindenAl.Models;

public class CategoryPropertyKey : BaseEntity
{
    public string? Name { get; set; } 
    public string? DataType { get; set; } 
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }    

    public ICollection<CategoryPropertyOption> CategoryPropertyOptions { get; set; }

    public CategoryPropertyKey()
    {
        CategoryPropertyOptions = new List<CategoryPropertyOption>();
    }
}