using System.Text.Json;
using SahibindenAl.DTOs;
using SahibindenAl.Models;
using SahibindenAl.Repository;


public class SavedSearchService : ISavedSearchService
{
    private readonly ISavedSearchRepository _savedSearchRepository;

    public SavedSearchService(ISavedSearchRepository savedSearchRepository)
    {
        _savedSearchRepository = savedSearchRepository;
    }

    public async Task SaveSearchAsync(int userId, string name, AdvertFilterDto filter)
    {
        var jsonQuery = JsonSerializer.Serialize(filter);

        var savedSearch = new SavedSearch
        {
            UserId = userId,
            Name = name,
            Query = jsonQuery,
            CreatedDate = DateTime.UtcNow
        };

        await _savedSearchRepository.AddAsync(savedSearch);
        await _savedSearchRepository.SaveAsync();
    }

    public async Task<List<SavedSearch>> GetUserSavedSearchesAsync(int userId)
    {
        return await _savedSearchRepository.GetByUserIdAsync(userId);
    }

    public async Task DeleteSearchAsync(int id)
    {
        var entity = await _savedSearchRepository.GetByIdAsync(id);
        if (entity != null)
        {
            _savedSearchRepository.Remove(entity);
            await _savedSearchRepository.SaveAsync();
        }
    }
}