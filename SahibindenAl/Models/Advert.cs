namespace SahibindenAl.Models;

public class Advert : BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int CityId { get; set; }
    public City? City { get; set; }

    public int DistrictId { get; set; }
    public District? District { get; set; }

    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<AdvertImage> AdvertImages { get; set; }
    public ICollection<AdvertPropertyValue> AdvertPropertyValues { get; set; }
    
    public Advert()
    {
        Favorites = new List<Favorite>();
        AdvertImages = new List<AdvertImage>();
        AdvertPropertyValues = new List<AdvertPropertyValue>();
    }
}