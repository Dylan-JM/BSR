using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSR.Controllers;

public class HomesController : Controller
{
    private readonly HomeService _homeService;

    public HomesController(HomeService homeService)
    {
        _homeService = homeService;
    }

    // HomeList Page
    public IActionResult Index(int? minPrice, int? maxPrice, int? minArea, int? maxArea)
    {
        var homesViewModel = new HomeViewModel();

        try
        {
            var homes = homesViewModel.Homes = _homeService.GetHomes();

            if (minPrice.HasValue)
            {
                homes = homes.Where(h => h.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                homes = homes.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            if (minArea.HasValue)
            {
                homes = homes.Where(h => h.Area >= minArea.Value).ToList();
            }

            if (maxArea.HasValue)
            {
                homes = homes.Where(h => h.Area <= maxArea.Value).ToList();
            }

            homesViewModel.Homes = homes;
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error fetching homes: {e.Message}";
        }

        homesViewModel.MaxPrice = maxPrice;
        homesViewModel.MinPrice = minPrice;

        homesViewModel.MaxArea = maxArea;
        homesViewModel.MinArea = minArea;

        return View(homesViewModel);
    }

    // Add Home
    [HttpGet]
    public IActionResult AddHomeView()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddHome(Home newHome)
    {
        if (!ModelState.IsValid)
        {
            return View("AddHomeView", newHome);
        }

        try
        {
            _homeService.AddHome(newHome);
            TempData["SuccessMessage"] = "Home added successfully";
            return RedirectToAction("Index", "Homes");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error adding home: {e.Message}";
            return View("AddHomeView", newHome);
        }
    }

    // Update Home
    [HttpGet]
    public IActionResult HomeDetailView(int id)
    {
        var home = _homeService.GetHomeById(id);
        return View(home);
    }

    [HttpPost]
    public IActionResult Update(Home updatedHome)
    {
        if (!ModelState.IsValid)
        {
            return View("HomeDetailView", updatedHome);
        }

        try
        {
            _homeService.UpdateHome(updatedHome);
            TempData["SuccessMessage"] = "Home updated successfully";
            return RedirectToAction("Index", "Homes");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error updating home: {e.Message}";
            return View("HomeDetailView", updatedHome);
        }
    }

    // Delete Home
    public IActionResult Delete(int id)
    {
        try
        {
            _homeService.DeleteHome(id);
            TempData["SuccessMessage"] = "Home deleted successfully";
            return new OkResult();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error deleting home: {e.Message}";
            return BadRequest(new { message = e.Message });
        }
    }
}
