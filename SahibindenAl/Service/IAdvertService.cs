using SahibindenAl.DTOs;
using SahibindenAl.Models;     

namespace SahibindenAl.Service;

public interface IAdvertService
{
    Task CreateAdvertAsync(AdvertCreateDto dto);

    Task<Advert?> GetAdvertByIdAsync(int id);

    Task<List<Advert>> GetAdvertsByCategoryAsync(int categoryId);

    Task<List<CategoryPropertyKey>> GetCategoryPropertiesAsync(int categoryId);

    Task<List<Advert>> GetFilteredAdvertsAsync(AdvertFilterDto filter);

    Task<bool> ToggleFavoriteAsync(int userId, int advertId);

    Task<IEnumerable<Category>> GetSubCategoriesAsync();

    Task<IEnumerable<City>> GetAllCitiesAsync();

    Task<IEnumerable<District>> GetDistrictsByCityIdAsync(int cityId);

    Task<List<Advert>> GetAdvertsByUserIdAsync(int userId);

    Task<List<Advert>> GetFavoriteAdvertsByUserIdAsync(int userId);

    Task UpdateAdvertAsync(AdvertUpdateDto dto);

    Task<bool> IsAdvertFavoriteAsync(int userId, int advertId);

    
}