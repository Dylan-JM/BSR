using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSR.Pages;

public class AddHome : PageModel
{
    private readonly HomeService _homeService;

    public AddHome(HomeService homeService)
    {
        _homeService = homeService;
    }

    [BindProperty]
    public Home NewHome { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        _homeService.AddHome(NewHome);
        return RedirectToPage("./Index");
    }
}
