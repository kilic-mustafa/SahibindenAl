using SahibindenAl.DTOs;
using SahibindenAl.Models;
using SahibindenAl.Service;
using Microsoft.AspNetCore.Mvc;
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


    
    [HttpGet]
    public async Task<IActionResult> GetCategoryProperties(int categoryId)
    {
        
        var properties = await _advertService.GetCategoryPropertiesAsync(categoryId);

        
        return PartialView("_DynamicProperties", properties);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdvertCreateDto dto, Dictionary<int, string>? dynamicProps)
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
            
            if (dynamicProps != null)
            {
                foreach (var item in dynamicProps)
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
}