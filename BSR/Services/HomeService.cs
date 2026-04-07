using BSR.Models;

namespace BSR.Services;

public class HomeService
{
    private readonly HomeContext _context;
    private List<Home> _homes;

    public HomeService(HomeContext context)
    {
        _context = context;
    }

    public List<Home> GetHomes()
    {
        return _context.Homes.ToList();
    }

    public void AddHome(Home home)
    {
        _context.Add(home);
        _context.SaveChanges();
    }

    public Home GetHomeById(int id)
    {
        return _context.Homes.Single(h => h.Id == id);
    }

    public void DeleteHome(int id)
    {
        var home = _context.Homes.FirstOrDefault(h => h.Id == id);
        _context.Homes.Remove(home);
        _context.SaveChanges();
    }

    public void UpdateHome(Home updatedHome)
    {
        var home = _context.Homes.FirstOrDefault(h => h.Id == updatedHome.Id);

        home.Price = updatedHome.Price;
        home.Address = updatedHome.Address;
        home.Area = updatedHome.Area;

        _context.Homes.Update(home);
        _context.SaveChanges();
    }
}
