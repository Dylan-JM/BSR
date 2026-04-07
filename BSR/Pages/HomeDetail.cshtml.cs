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

    [BindProperty]
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

    public IActionResult OnPostUpdate()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _homeService.UpdateHome(Home);
            TempData["SuccessMessage"] = "Successfully updated home";
            return RedirectToPage("/Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error updating home: {e.Message}";
            return Page();
        }
    }

    public IActionResult OnPostDelete(int id)
    {
        try
        {
            _homeService.DeleteHome(id);
            TempData["SuccessMessage"] = "Successfully deleted home";
            return new OkResult();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error deleting home: {e.Message}";
            return Page();
        }
    }
}
