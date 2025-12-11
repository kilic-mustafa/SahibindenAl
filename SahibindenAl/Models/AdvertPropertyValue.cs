namespace SahibindenAl.Models;

public class AdvertPropertyValue : BaseEntity
{
    public int AdvertId { get; set; }
    public Advert? Advert { get; set; }

    public int CategoryPropertyKeyId { get; set; }
    public CategoryPropertyKey? PropertyKey { get; set; }

    public string? Value { get; set; } 
}