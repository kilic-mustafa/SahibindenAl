using SahibindenAl.Data;
using SahibindenAl.DTOs;
using SahibindenAl.Models;
using Microsoft.EntityFrameworkCore;

namespace SahibindenAl.Repository;

public class AdvertRepository : GenericRepository<Advert>, IAdvertRepository
{
    public AdvertRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Advert?> GetAdvertWithDetailsAsync(int id)
    {
        return await _context.Adverts
            .Include(x => x.Category)
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.User)
            .Include(x => x.AdvertImages)
            .Include(x => x.AdvertPropertyValues)
                .ThenInclude(y => y.PropertyKey)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Advert>> GetAdvertsByCategoryWithDetailsAsync(int categoryId)
    {
        return await _context.Adverts
            .Where(x => x.CategoryId == categoryId && x.IsActive && !x.IsDeleted)
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.AdvertImages)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<Advert>> GetAdvertsByFilterAsync(AdvertFilterDto filter)
    {
        var query = _context.Adverts
            .Include(x => x.City)           
            .Include(x => x.District)
            .Include(x => x.AdvertImages)   
            .Where(x => x.IsActive && !x.IsDeleted) 
            .AsQueryable();
        
        if (filter.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
        }

        if (filter.CityId.HasValue)
        {
            query = query.Where(x => x.CityId == filter.CityId.Value);
        }

        if (filter.DistrictId.HasValue)
        {
            query = query.Where(x => x.DistrictId == filter.DistrictId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(x => x.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(x => x.Price <= filter.MaxPrice.Value);
        }

        if (!string.IsNullOrEmpty(filter.SearchText))
        {
            var text = filter.SearchText;

            query = query.Where(x =>
                (x.Title != null && EF.Functions.ILike(x.Title, $"%{text}%")) ||
                (x.Description != null && EF.Functions.ILike(x.Description, $"%{text}%")) ||
                (x.Id.ToString() == text) ||
                (x.User != null && EF.Functions.ILike(x.User.FirstName + " " + x.User.LastName, $"%{text}%"))         
                );
        }

        query = query.OrderByDescending(x => x.CreatedDate); 

        return await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task CreateAdvertAsync(Advert advert, List<AdvertPropertyValue> propertyValues)
    {
        await _context.Adverts.AddAsync(advert);
        await _context.SaveChangesAsync();
        
        foreach (var propValue in propertyValues)
        {
            propValue.AdvertId = advert.Id;
            await _context.AdvertPropertyValues.AddAsync(propValue);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<List<CategoryPropertyKey>> GetCategoryPropertiesAsync(int categoryId)
    {
        return await _context.CategoryPropertyKeys
            .Include(x => x.CategoryPropertyOptions)
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryPropertyOption>> GetChildOptionsAsync(int parentOptionId)
    {
        return await _context.CategoryPropertyOptions
            .Where(x => x.Id == parentOptionId)
            .ToListAsync();
    }

    public async Task<List<Advert>> GetAdvertsByUserIdWithDetailsAsync(int userId)
    {
        return await _context.Adverts
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .Include(x => x.Category)
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.AdvertImages)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task UpdateAdvertAsync(Advert advert, List<AdvertPropertyValue> newPropertyValues)
    {
        var oldValues = await _context.AdvertPropertyValues
            .Where(x => x.AdvertId == advert.Id)
            .ToListAsync();
        
        _context.AdvertPropertyValues.RemoveRange(oldValues);

        foreach (var val in newPropertyValues)
        {
            val.AdvertId = advert.Id;
            await _context.AdvertPropertyValues.AddAsync(val);
        }

        _context.Adverts.Update(advert);
        
        await _context.SaveChangesAsync();
    }
}