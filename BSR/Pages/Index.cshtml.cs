using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSR.Pages;

public class Index : PageModel
{
    private readonly HomeService _homeService;

    public Index(HomeService homeService)
    {
        _homeService = homeService;
    }

    public List<Home> Homes { get; private set; }
    public decimal ThresholdPrice { get; set; }

    public void OnGet()
    {
        try
        {
            Homes = _homeService.GetHomes();
            ThresholdPrice = 400000;
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error fetching homes from the database: {e.Message}. Refresh the page";
        }
    }
}
