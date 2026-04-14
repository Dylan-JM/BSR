using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSR.Controllers;

[Authorize(Roles ="Admin")]
public class UsersController : Controller
{
    private readonly ILogger<HomesController> _logger;
    private readonly UserService _userService;

    public UsersController(ILogger<HomesController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var usersViewModel = await _userService.GetUsers();

        ViewBag.UsersCount = usersViewModel.Users.Count - 1 ; 

        return View(usersViewModel);
    }

    [HttpGet]
    public IActionResult UserDetailView(string id)
    {
        try
        {
            var user = _userService.GetUserById(id);
            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching user details for ID: {id}");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost]
    public IActionResult Update(UserViewModel user)
    {
        try
        {
            _userService.UpdateUser(user);
            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction("Index", "Users");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user {ex.Message}");
            TempData["ErrorMessage"] = $"Error updating user: {ex.Message}";
            return View("UserDetailView", user);
        }
    }

}
