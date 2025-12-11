namespace SahibindenAl.Models;

public class City : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<District> Districts { get; set; }

    public City()
    {
        Districts = new List<District>();
    }
}