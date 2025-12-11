namespace SahibindenAl.Models;

public class Favorite : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int AdvertId { get; set; }
    public Advert? Advert { get; set; }
}