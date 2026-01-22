using SahibindenAl.DTOs;
using SahibindenAl.Models;
using SahibindenAl.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SahibindenAl.Controllers;

[Authorize] 
public class AdvertController : Controller
{
    private readonly IAdvertService _advertService;
    private readonly UserManager<User> _userManager;

    public AdvertController(IAdvertService advertService, UserManager<User> userManager)
    {
        _advertService = advertService;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> Details(int id)
    {
        var advert = await _advertService.GetAdvertByIdAsync(id);

        if (advert == null)
        {
            return NotFound();
        }

        bool isFavorite = false;
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            
            isFavorite = await _advertService.IsAdvertFavoriteAsync(user.Id, id);
        }
        
        ViewBag.IsFavorite = isFavorite;
        return View(advert);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        
        await PopulateDropdownsAsync();

        return View();
    }

    
    [HttpGet]
    public async Task<IActionResult> GetDistrictsByCity(int cityId)
    {
        var districts = await _advertService.GetDistrictsByCityIdAsync(cityId);
        return Json(districts);
    }

    private async Task PopulateDropdownsAsync()
    {
        ViewBag.Categories = await _advertService.GetSubCategoriesAsync();
        ViewBag.Cities = await _advertService.GetAllCitiesAsync();
    }


    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdvertCreateDto dto, Dictionary<int, string>? PropertyValues)
    {
        if (!ModelState.IsValid)
        {
            
            await PopulateDropdownsAsync();
            return View(dto);
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                
                return Challenge(); 
            }

            dto.UserId = user.Id;

            dto.DynamicProperties = new List<AdvertPropertyInput>();
            
            if (PropertyValues != null)
            {
                foreach (var item in PropertyValues)
                {
                    dto.DynamicProperties.Add(new AdvertPropertyInput
                    {
                        PropertyKeyId = item.Key,
                        Value = item.Value
                    });
                }
            }
            
            await _advertService.CreateAdvertAsync(dto);

            TempData["SuccessMessage"] = "İlanınız başarıyla oluşturuldu!";
            
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "İlan oluşturulurken beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            await PopulateDropdownsAsync();
            return View(dto);
        }
    }

    public async Task<IActionResult> MyAdverts()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var myAdverts = await _advertService.GetAdvertsByUserIdAsync(user.Id);

        return View(myAdverts);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var advert = await _advertService.GetAdvertByIdAsync(id);
        if (advert == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (user == null || advert.UserId != user.Id)
        {
            return Forbid();
        }

        var dto = new AdvertUpdateDto
        {
            Id = advert.Id,
            Title = advert.Title,
            Description = advert.Description,
            Price = advert.Price,
            CategoryId = advert.CategoryId,
            CityId = advert.CityId,
            DistrictId = advert.DistrictId,
            UserId = advert.UserId
        };

        ViewBag.AdvertImages = advert.AdvertImages;

        await PopulateDropdownsAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, AdvertUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync();
            var advert = await _advertService.GetAdvertByIdAsync(id);
            if (advert != null)
            {
                dto.Title = advert.Title;
                dto.Description = advert.Description;
                dto.Price = advert.Price;
                dto.CategoryId = advert.CategoryId;
                dto.CityId = advert.CityId;
                dto.DistrictId = advert.DistrictId;
            }
            return View(dto);
        }

        try
        {
            await _advertService.UpdateAdvertAsync(dto);
            TempData["SuccessMessage"] = "İlan başarıyla güncellendi.";
            return RedirectToAction(nameof(MyAdverts));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
            await PopulateDropdownsAsync();
            return View(dto);
        }
    }   

    [HttpGet]
    public async Task<IActionResult> GetCategoryProperties(int categoryId, int? advertId = null)
    {
        try
        {
            var properties = await _advertService.GetCategoryPropertiesAsync(categoryId);
            
            if (advertId.HasValue && advertId > 0)
            {
                var advert = await _advertService.GetAdvertByIdAsync(advertId.Value);
                ViewBag.ExistingValues = advert?.AdvertPropertyValues?.ToList() ?? new List<AdvertPropertyValue>();
            }

            return PartialView("_DynamicProperties", properties);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error loading properties: {ex.Message}");
        }
    }

}