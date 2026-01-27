namespace SahibindenAl.Models
{
    public class SavedSearch : BaseEntity
    {
        public string? Name { get; set; }
        public string? Query { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}