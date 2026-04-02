using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSR.Pages;

public class HomeDetail : PageModel
{
    private readonly HomeService _homeService;

    public HomeDetail(HomeService homeService)
    {
        _homeService = homeService;
    }

    public Home Home { get; set; }

    public IActionResult OnGet(int id)
    {
        Home = GetHomeById(id);
        return Page();
    }

    private Home GetHomeById(int id)
    {
        return _homeService.GetHomeById(id);
    }
}
