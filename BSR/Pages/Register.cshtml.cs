using System.ComponentModel.DataAnnotations;
using BSR.Views.Homes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BSR.Controllers;
namespace BSR.Pages;

public class Register : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomesController> _logger;

    public Register(
        ILogger<HomesController> logger,
        SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public RegisterInputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            try
            {
                var identity = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
                };
                var result = await _userManager.CreateAsync(identity, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identity, "User");
                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return LocalRedirect("~/");
                }
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred in OnPostExternalLogin: {ex.Message}");
                TempData["ErrorMessage"] = "There was an error with the configuration of external authentication properties";
                return RedirectToPage("/Index");
            }
        }

        return Page();
    }

    public IActionResult OnPostExternalLogin()
    {
        try
        {
            var returnUrl = Url.Page("./Register", pageHandler: "Callback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", returnUrl);

            return Challenge(properties, "Microsoft");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred in OnPostExternalLogin: {ex.Message}");
            TempData["ErrorMessage"] = "There was an error with the configuration of external authentication properties";

            return RedirectToPage("/Index");
        }
    }

    public async Task<IActionResult> OnGetCallbackAsync()
    {
        try
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                TempData["ErrorMessage"] = "External login information is not available.";
                return RedirectToPage("/Index");
            }

            var emailClaim = info.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");

            var identity = new ApplicationUser
            {
                UserName = emailClaim.Value ?? "",
                Email = emailClaim.Value ?? "",
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                await _userManager.CreateAsync(identity);
            }

            await _signInManager.SignInAsync(identity, isPersistent: false, info.LoginProvider);

            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred in OnGetCallbackAsync: {ex.Message}");
            TempData["ErrorMessage"] = "There was an error with the external login";
            return RedirectToPage("/Index");
        }
    }
}

public class RegisterInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}