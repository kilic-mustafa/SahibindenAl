namespace SahibindenAl.Models;

public class AdvertImage : BaseEntity
{
    public string? ImageUrl { get; set; }
    public bool IsCoverImage { get; set; } 
    
    public int AdvertId { get; set; }
    public Advert? Advert { get; set; }
}