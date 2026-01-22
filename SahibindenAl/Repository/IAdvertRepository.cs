using SahibindenAl.DTOs;
using SahibindenAl.Models;

namespace SahibindenAl.Repository;

public interface IAdvertRepository : IGenericRepository<Advert>
{
    Task<Advert?> GetAdvertWithDetailsAsync(int id);
    Task<List<Advert>> GetAdvertsByCategoryWithDetailsAsync(int categoryId);
    Task<List<Advert>> GetAdvertsByFilterAsync(AdvertFilterDto filter);
    Task CreateAdvertAsync(Advert advert, List<AdvertPropertyValue> advertPropertyValues);
    Task<List<CategoryPropertyKey>> GetCategoryPropertiesAsync(int categoryId);
    Task<IEnumerable<CategoryPropertyOption>> GetChildOptionsAsync(int parentOptionId);
    Task<List<Advert>> GetAdvertsByUserIdWithDetailsAsync(int userId);
    Task UpdateAdvertAsync(Advert advert, List<AdvertPropertyValue> newPropertyValues);
    Task<bool> ToggleFavoriteAsync(int userId, int advertId);
    Task<List<Advert>> GetFavoriteAdvertsByUserIdAsync(int userId);
    Task<bool> AnyFavoriteAsync(int userId, int advertId);
}
