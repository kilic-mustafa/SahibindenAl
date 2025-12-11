namespace SahibindenAl.DTOs;

public class AdvertFilterDto
{
    public int? CategoryId { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SearchText { get; set; } 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}