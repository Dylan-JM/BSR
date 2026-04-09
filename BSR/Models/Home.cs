using System.ComponentModel.DataAnnotations;

namespace BSR.Models;

public class Home
{
    public int Id { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int GarageSpots { get; set; }
    public decimal Price { get; set; }
    public int Area { get; set; }
    public string? ImageUrl { get; set; }
}
