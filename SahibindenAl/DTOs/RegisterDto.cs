using System.ComponentModel.DataAnnotations;

namespace SahibindenAl.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 30 karakter arasında olmalıdır.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Ad zorunludur.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Soyad zorunludur.")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
    public string? ConfirmPassword { get; set; }
}
