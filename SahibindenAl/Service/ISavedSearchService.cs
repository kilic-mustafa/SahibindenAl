using SahibindenAl.DTOs;
using SahibindenAl.Models;     

public interface ISavedSearchService
{
    Task SaveSearchAsync(int userId, string name, AdvertFilterDto filter);
    Task<List<SavedSearch>> GetUserSavedSearchesAsync(int userId);
    Task DeleteSearchAsync(int id);
}