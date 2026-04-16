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
                    ConcurrencyStamp = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                },
                new IdentityRole
                {
                    Id = "sales",
                    Name = "Sales",
                    NormalizedName = "SALES",
                    ConcurrencyStamp = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
                },
                new IdentityRole
                {
                    Id = "user",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "c3d4e5f6-a7b8-9012-cdef-123456789012",
                }
            );
    }
}
