using SahibindenAl.DTOs;
using SahibindenAl.Service;
using Microsoft.AspNetCore.Mvc;

namespace SahibindenAl.Controllers;

public class DetailedSearchController : Controller
{
    private readonly IAdvertService _advertService;

    public DetailedSearchController(IAdvertService advertService)
    {
        _advertService = advertService;
    }

    public async Task<IActionResult> Index(AdvertFilterDto filter)
    {
        var adverts = await _advertService.GetFilteredAdvertsAsync(filter);

        ViewBag.Cities = await _advertService.GetAllCitiesAsync();
        ViewBag.Categories = await _advertService.GetSubCategoriesAsync();

        if (filter.CityId.HasValue)
        {
            ViewBag.Districts = await _advertService.GetDistrictsByCityIdAsync(filter.CityId.Value);
        }
        else
        {
            ViewBag.Districts = new List<Models.District>();
        } 

        ViewBag.CurrentFilter = filter;

        return View(adverts);
    }

    [HttpGet]
    public async Task<IActionResult> GetDistricts(int cityId)
    {
        var districts = await _advertService.GetDistrictsByCityIdAsync(cityId);
        return Json(districts.Select(d => new { id = d.Id, name = d.Name }));
    }
    
}