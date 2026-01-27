
using SahibindenAl.Data;
using SahibindenAl.Models;
using Microsoft.EntityFrameworkCore;

namespace SahibindenAl.Repository;

public class SavedSearchRepository : GenericRepository<SavedSearch>, ISavedSearchRepository
{
    public SavedSearchRepository(AppDbContext context) : base(context) { }

    public async Task<List<SavedSearch>> GetByUserIdAsync(int userId)
    {
        return await _context.SavedSearches
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
}