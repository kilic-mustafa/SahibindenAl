using System.ComponentModel.DataAnnotations;

namespace SahibindenAl.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}
