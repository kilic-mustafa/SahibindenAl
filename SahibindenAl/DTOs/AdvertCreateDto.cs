using System.ComponentModel.DataAnnotations;

namespace SahibindenAl.DTOs;

public class AdvertCreateDto
{
    [Required(ErrorMessage = "İlan başlığı boş bırakılamaz.")]
    [StringLength(100, ErrorMessage = "İlan başlığı en fazla 100 karakter olabilir.")]
    public string? Title { get; set; }

    [StringLength(5000, ErrorMessage = "Açıklama en fazla 5000 karakter olabilir.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Fiyat boş bırakılamaz.")]
    [Range(1, 100000000, ErrorMessage = "Lütfen geçerli bir fiyat giriniz.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçiniz.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Şehir seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir şehir seçiniz.")]
    public int CityId { get; set; }

    [Required(ErrorMessage = "İlçe seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir ilçe seçiniz.")]
    public int DistrictId { get; set; }
    
    public int UserId { get; set; }

    public List<IFormFile>? Photos { get; set; } = null;
    public List<AdvertPropertyInput>? DynamicProperties { get; set; } = null;
}

public class AdvertPropertyInput
{
    public int PropertyKeyId { get; set; }
    public string? Value { get; set; }
}