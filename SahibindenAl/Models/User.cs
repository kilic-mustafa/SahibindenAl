using Microsoft.AspNetCore.Identity;

namespace SahibindenAl.Models;

public class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public ICollection<Advert> Adverts { get; set; } 
    public ICollection<Favorite> Favorites { get; set; }

    public User()
    {
        Adverts = new List<Advert>();
        Favorites = new List<Favorite>();
    }
}