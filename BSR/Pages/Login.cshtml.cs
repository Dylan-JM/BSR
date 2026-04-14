using System.ComponentModel.DataAnnotations;
using BSR.Views.Homes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSR.Controllers;

namespace BSR.Pages;

public class Login : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<HomesController> _logger;

    public Login(
        SignInManager<ApplicationUser> signInManager, 
        ILogger<HomesController> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect("~/");
                }
                else
                {
                    _logger.LogError($"Login Unsuccessful");
                    TempData["ErrorMessage"] = "Login Not Successful";
                }
            }
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Login error: {ex.Message}");
            TempData["ErrorMessage"] = "There was an error";
            return Page();
        }

        return Page();
    }
}

public class LoginInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}