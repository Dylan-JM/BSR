using System.Diagnostics;
using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSR.Controllers;

public class HomesController : Controller
{
    private readonly ILogger<HomesController> _logger;
    private readonly HomeService _homeService;
    private readonly AddressService _addressService;

    public HomesController(
        ILogger<HomesController> logger,
        HomeService homeService,
        AddressService addressService
    )
    {
        _logger = logger;
        _homeService = homeService;
        _addressService = addressService;
    }

    public async Task<IActionResult> GetCities(string county)
    {
        var cities = await _addressService.GetCitiesInCounty(county);
        return Ok(cities);
    }

    [HttpGet]
    public async Task<IActionResult> AddHomeView()
    {
        var counties = await _addressService.GetUkCounties();

        var vm = new AddHomeViewModel
        {
            Counties = counties.Keys.ToList(),
            Cities = new List<string>(),
        };

        ViewBag.CountyCodes = counties;

        return View(vm);
    }

    // HomeList Page
    public IActionResult Index(
        int? minPrice,
        int? maxPrice,
        int? minArea,
        int? maxArea,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var homesViewModel = new HomeViewModel();

        try
        {
            var homes = _homeService.GetHomes();

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

            int totalItems = homes.Count();
            homes = homes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            homesViewModel.PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = pageSize,
                TotalItems = totalItems,
            };

            // demo to see logs
            if (totalItems > 450)
            {
                _logger.LogWarning("Database is close to reaching its capacity");
            }

            homesViewModel.Homes = homes;
            ViewBag.HomesCount = homes.Count();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching homes from the database {EMessage}", e.Message);
            TempData["ErrorMessage"] = $"Error fetching homes: {e.Message}";
        }

        homesViewModel.MaxPrice = maxPrice;
        homesViewModel.MinPrice = minPrice;

        homesViewModel.MaxArea = maxArea;
        homesViewModel.MinArea = minArea;

        stopwatch.Stop();

        ViewBag.LoadTestTime = stopwatch.Elapsed.TotalSeconds.ToString("F4");

        return View(homesViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddHome(Home newHome)
    {
        if (!ModelState.IsValid)
        {
            var counties = await _addressService.GetUkCounties();
            var vm = new AddHomeViewModel
            {
                Id = newHome.Id,
                StreetAddress = newHome.StreetAddress,
                City = newHome.City,
                County = newHome.County,
                Bedrooms = newHome.Bedrooms,
                Bathrooms = newHome.Bathrooms,
                GarageSpots = newHome.GarageSpots,
                Price = newHome.Price,
                Area = newHome.Area,
                Counties = counties.Keys.ToList(),
                Cities =
                    !string.IsNullOrEmpty(newHome.County)
                    && counties.TryGetValue(newHome.County, out var selectedCode)
                        ? await _addressService.GetCitiesInCounty(selectedCode)
                        : new List<string>(),
            };

            ViewBag.CountyCodes = counties;
            return View("AddHomeView", vm);
        }

        try
        {
            _homeService.AddHome(newHome);
            TempData["SuccessMessage"] = "Home added successfully";
            return RedirectToAction("Index", "Homes");
        }
        catch (Exception e)
        {
            var counties = await _addressService.GetUkCounties();
            var vm = new AddHomeViewModel
            {
                Id = newHome.Id,
                StreetAddress = newHome.StreetAddress,
                City = newHome.City,
                County = newHome.County,
                Bedrooms = newHome.Bedrooms,
                Bathrooms = newHome.Bathrooms,
                GarageSpots = newHome.GarageSpots,
                Price = newHome.Price,
                Area = newHome.Area,
                Counties = counties.Keys.ToList(),
                Cities =
                    !string.IsNullOrEmpty(newHome.County)
                    && counties.TryGetValue(newHome.County, out var selectedCode)
                        ? await _addressService.GetCitiesInCounty(selectedCode)
                        : new List<string>(),
            };

            ViewBag.CountyCodes = counties;
            TempData["ErrorMessage"] = $"Error adding home: {e.Message}";
            return View("AddHomeView", vm);
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
