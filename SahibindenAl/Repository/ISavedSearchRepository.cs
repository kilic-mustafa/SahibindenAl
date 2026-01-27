using SahibindenAl.Models;

namespace SahibindenAl.Repository;

public interface ISavedSearchRepository : IGenericRepository<SavedSearch>
{
    Task<List<SavedSearch>> GetByUserIdAsync(int userId);
}