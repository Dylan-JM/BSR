using BSR.Views.Homes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSR.Models;

public class HomeContext : IdentityDbContext<ApplicationUser>
{
    public HomeContext(DbContextOptions<HomeContext> options)
        : base(options) { }

    public DbSet<Home> Homes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = "admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Id = "sales",
                    Name = "Sales",
                    NormalizedName = "SALES",
                },
                new IdentityRole
                {
                    Id = "user",
                    Name = "User",
                    NormalizedName = "USER",
                }
            );
    }
}
