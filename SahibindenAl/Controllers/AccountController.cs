using SahibindenAl.DTOs;
using SahibindenAl.Models;
using SahibindenAl.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace SahibindenAl.Controllers;
public class AccountController : Controller
{
    private readonly IAdvertService _advertService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(IAdvertService advertService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _advertService = advertService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> AccountSettings()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    public async Task<IActionResult> MyFavorites()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var favoriteAdverts = await _advertService.GetFavoriteAdvertsByUserIdAsync(user.Id);

        return View(favoriteAdverts);
    }    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAccount(User updatedUser)
    {
        var user = await _userManager.FindByIdAsync(updatedUser.Id.ToString());
        
        if (user == null) return NotFound();

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.UserName = updatedUser.UserName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Bilgileriniz başarıyla güncellendi.";
            return RedirectToAction(nameof(AccountSettings)); 
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View("AccountSettings", updatedUser);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.UserName
        };

        var result = await _userManager.CreateAsync(user, dto.Password!);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        
        return View(dto);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
    {        
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _signInManager.PasswordSignInAsync(dto.UserName!, dto.Password!, dto.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(int advertId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized(new { message = "Lütfen önce giriş yapın." });
        }

        try
        {
            var isAdded = await _advertService.ToggleFavoriteAsync(user.Id, advertId);

            return Ok(new { success = true, isAdded = isAdded });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "İşlem sırasında bir hata oluştu.", detail = ex.Message });
        }
    }
       
}