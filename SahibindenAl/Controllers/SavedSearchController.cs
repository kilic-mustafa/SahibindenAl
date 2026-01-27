using SahibindenAl.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SahibindenAl.Models;
using System.Security.Claims;

[Route("[controller]")]
public class SavedSearchController : Controller
{
    private readonly ISavedSearchService _savedSearchService;
    private readonly UserManager<User> _userManager;

    public SavedSearchController(ISavedSearchService savedSearchService, UserManager<User> userManager)
    {
        _savedSearchService = savedSearchService;
        _userManager = userManager;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var savedSearches = await _savedSearchService.GetUserSavedSearchesAsync(int.Parse(userId));

        return View(savedSearches);
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _savedSearchService.DeleteSearchAsync(id);

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("Execute/{id}")]
    public async Task<IActionResult> Execute(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var allSearches = await _savedSearchService.GetUserSavedSearchesAsync(int.Parse(userId));
        var selectedSearch = allSearches.FirstOrDefault(x => x.Id == id);

        if (selectedSearch == null || string.IsNullOrEmpty(selectedSearch.Query)) return NotFound();

        var filter = System.Text.Json.JsonSerializer.Deserialize<SahibindenAl.DTOs.AdvertFilterDto>(selectedSearch.Query);

        return RedirectToAction("Index", "Home", filter);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSearches(int userId)
    {
        var result = await _savedSearchService.GetUserSavedSearchesAsync(userId);
        return Ok(result);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveSearch([FromBody] SaveSearchRequest request)
    {
        if (string.IsNullOrEmpty(request.Name) || request.Filter == null)
        {
            return BadRequest("Arama adı ve filtre boş olamaz.");
        }
        await _savedSearchService.SaveSearchAsync(request.UserId, request.Name, request.Filter);
        return Ok(new { message = "Arama başarıyla kaydedildi." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSearch(int id)
    {
        await _savedSearchService.DeleteSearchAsync(id);
        return Ok(new { message = "Kayıt silindi." });
    }
}

public class SaveSearchRequest
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public AdvertFilterDto? Filter { get; set; }
}