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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _homeService.AddHome(NewHome);
            TempData["SuccessMessage"] = "Home added successfully";
            return RedirectToPage("./Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error adding home: {e.Message}";
            return Page();
        }
    }
}
