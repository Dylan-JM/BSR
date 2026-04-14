using BSR.Models;
using BSR.Views.Homes;
using Microsoft.EntityFrameworkCore;

namespace BSR.Services;

public class UserService
{
    private readonly HomeContext _context;

    public UserService(HomeContext context)
    {
        _context = context;
    }

    public async Task<UsersViewModel> GetUsers()
    {
        var users = await (from u in _context.Users
                           join ur in _context.UserRoles on u.Id equals ur.UserId
                           join r in _context.Roles on ur.RoleId equals r.Id
                           select new UserViewModel
                           {
                               Id = u.Id,
                               Email = u.Email,
                               Role = r.Name,
                               RegistrationDate = u.RegistrationDate
                           })
                   .ToListAsync();

        return new UsersViewModel
        {
            Users = users
        };
    }

    public UserViewModel GetUserById(string id)
    {
        var user = (from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where u.Id == id
                    select new UserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Role = r.Name,
                        RegistrationDate = u.RegistrationDate
                    })
                  .SingleOrDefault();

        return user;
    }

    public void UpdateUser(UserViewModel user)
    {
        _context.UserRoles
            .Where(x => x.UserId == user.Id)
            .ExecuteUpdate(y => y.SetProperty(u => u.RoleId, user.Role.ToLower()));

        _context.SaveChanges();
    }
}
