using SahibindenAl.DTOs;
using SahibindenAl.Models;
using SahibindenAl.Repository;
using Microsoft.EntityFrameworkCore;

namespace SahibindenAl.Service;

public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IGenericRepository<District> _districtRepository;
    private readonly IWebHostEnvironment _env;

    public AdvertService(
        IAdvertRepository advertRepository,
        IWebHostEnvironment env,
        IGenericRepository<Category> categoryRepository,
        IGenericRepository<City> cityRepository,
        IGenericRepository<District> districtRepository)
    {
        _advertRepository = advertRepository;
        _env = env;
        _categoryRepository = categoryRepository;
        _cityRepository = cityRepository;
        _districtRepository = districtRepository;
    }

    public async Task CreateAdvertAsync(AdvertCreateDto dto)
    {
        var newAdvert = new Advert
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            CityId = dto.CityId,
            DistrictId = dto.DistrictId,
            UserId = dto.UserId,
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false,
            AdvertImages = new List<AdvertImage>()
        };

        var advertPropertyValues = new List<AdvertPropertyValue>();
        if (dto.DynamicProperties != null)
        {
            foreach (var prop in dto.DynamicProperties)
            {
                var newPropValue = new AdvertPropertyValue
                {
                    CategoryPropertyKeyId = prop.PropertyKeyId,
                    Value = prop.Value,
                    IsDeleted = false,
                    IsActive = true
                };
                advertPropertyValues.Add(newPropValue);
            }
        }

        if (dto.Photos != null && dto.Photos.Count > 0)
        {
            foreach (var file in dto.Photos)
            {
                string imagePath = await SaveFileAsync(file);

                newAdvert.AdvertImages.Add(new AdvertImage
                {
                    ImageUrl = imagePath,
                    IsCoverImage = false, 
                    IsActive = true,
                    IsDeleted = false
                });
            }

            if (newAdvert.AdvertImages.Any())
            {
                newAdvert.AdvertImages.First().IsCoverImage = true;
            }
        }

        await _advertRepository.CreateAdvertAsync(newAdvert, advertPropertyValues);
        await _advertRepository.SaveAsync();
    }

    private async Task<string> SaveFileAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return "/uploads/" + uniqueFileName;
    }
        
    public async Task<Advert?> GetAdvertByIdAsync(int id)
    {
        return await _advertRepository.GetAdvertWithDetailsAsync(id);
    }

    public async Task<List<Advert>> GetAdvertsByCategoryAsync(int categoryId)
    {
        return await _advertRepository.GetAdvertsByCategoryWithDetailsAsync(categoryId);
    }

    public async Task<List<CategoryPropertyKey>> GetCategoryPropertiesAsync(int categoryId)
    {
        return await _advertRepository.GetCategoryPropertiesAsync(categoryId);
    }

    public async Task<List<Advert>> GetFilteredAdvertsAsync(AdvertFilterDto filter)
    {
        return await _advertRepository.GetAdvertsByFilterAsync(filter);
    }
    
    public async Task ToggleFavoriteAsync(int userId, int advertId)
    {
        var advert = await _advertRepository.GetByIdAsync(advertId);
        if (advert == null)
        {
            throw new KeyNotFoundException("Advert not found");
        }

        var existingFavorite = advert.Favorites?.FirstOrDefault(f => f.UserId == userId);
        if (existingFavorite != null)
        {
            advert.Favorites!.Remove(existingFavorite);
        }
        else
        {
            advert.Favorites ??= new List<Favorite>();
            advert.Favorites.Add(new Favorite
            {
                UserId = userId,
                AdvertId = advertId
            });
        }

        _advertRepository.Update(advert);
        await _advertRepository.SaveAsync();
    }

    public async Task<IEnumerable<City>> GetAllCitiesAsync()
    {
        return await _cityRepository.GetAllAsync();
    }

    public async Task<IEnumerable<District>> GetDistrictsByCityIdAsync(int cityId)
    {
        return await _districtRepository.Where(d => d.CityId == cityId).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetSubCategoriesAsync()
    {
        return await _categoryRepository.Where(c => c.ParentCategoryId != null).ToListAsync();
    }

    public async Task<List<Advert>> GetAdvertsByUserIdAsync(int userId)
    {
        return await _advertRepository.GetAdvertsByUserIdWithDetailsAsync(userId);
    }

    public async Task UpdateAdvertAsync(int id, Advert updatedAdvert, List<IFormFile> newPhotos, Dictionary<int, string> dynamicProperties)
    {
        var existingAdvert = await _advertRepository.GetAdvertWithDetailsAsync(id);
        if (existingAdvert == null) throw new KeyNotFoundException("İlan bulunamadı.");

        existingAdvert.Title = updatedAdvert.Title;
        existingAdvert.Description = updatedAdvert.Description;
        existingAdvert.Price = updatedAdvert.Price;
        existingAdvert.CategoryId = updatedAdvert.CategoryId;
        existingAdvert.CityId = updatedAdvert.CityId;
        existingAdvert.DistrictId = updatedAdvert.DistrictId;
        existingAdvert.IsActive = updatedAdvert.IsActive;

        var propertyValues = new List<AdvertPropertyValue>();
        if (dynamicProperties != null)
        {
            foreach (var prop in dynamicProperties)
            {
                if (!string.IsNullOrEmpty(prop.Value))
                {
                    propertyValues.Add(new AdvertPropertyValue
                    {
                        CategoryPropertyKeyId = prop.Key,
                        Value = prop.Value,
                        AdvertId = id,
                        IsActive = true
                    });
                }
            }
        }

        if (newPhotos != null && newPhotos.Count > 0)
        {
            foreach (var file in newPhotos)
            {
                string imagePath = await SaveFileAsync(file);
                existingAdvert.AdvertImages.Add(new AdvertImage
                {
                    ImageUrl = imagePath,
                    IsCoverImage = false,
                    IsActive = true,
                    IsDeleted = false
                });
            }
        }

        await _advertRepository.UpdateAdvertAsync(existingAdvert, propertyValues);
    }
}