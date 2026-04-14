using Microsoft.AspNetCore.Identity;

namespace BSR.Views.Homes;

public class ApplicationUser : IdentityUser
{
     public DateOnly RegistrationDate { get; set; }
}
