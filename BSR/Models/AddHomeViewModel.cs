using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BSR;

public class AddHomeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Address field is required.")]
    public string StreetAddress { get; set; }

    public string City { get; set; }
    public string County { get; set; }

    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int GarageSpots { get; set; }

    [Required(ErrorMessage = "The Price field is required.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The Area field is required.")]
    public int Area { get; set; }

    [ValidateNever]
    public string? ImageUrl { get; set; }

    public List<string> Counties { get; set; }
    public List<string> Cities { get; set; }
}
