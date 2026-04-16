using BSR.Models;
using BSR.Services;
using BSR.Views.Homes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});

builder
    .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllersWithViews();

builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<HomeContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<DataSeedService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<HomeContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddAuthentication()
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder
            .Configuration.GetSection("MicrosoftAuth:ClientId")
            .Value;
        microsoftOptions.ClientSecret = builder
            .Configuration.GetSection("MicrosoftAuth:ClientSecret")
            .Value;
    });

var app = builder.Build();

app.UseHttpsRedirection();

await SeedRolesAsync(app);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Homes}/{action=Index}/{id?}"
    );
});

app.Run();

static async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<HomeContext>();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(
            new IdentityRole
            {
                Id = "admin",
                Name = "Admin",
                NormalizedName = "ADMIN",
            }
        );
    }
    if (!await roleManager.RoleExistsAsync("Sales"))
    {
        await roleManager.CreateAsync(
            new IdentityRole
            {
                Id = "sales",
                Name = "Sales",
                NormalizedName = "SALES",
            }
        );
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(
            new IdentityRole
            {
                Id = "user",
                Name = "User",
                NormalizedName = "USER",
            }
        );
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var adminUser = new ApplicationUser { UserName = "admin@bsr.com", Email = "admin@bsr.com" };
    var result = await userManager.CreateAsync(adminUser, "Admin123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    var salesUser = new ApplicationUser { UserName = "sales@bsr.com", Email = "sales@bsr.com" };
    result = await userManager.CreateAsync(salesUser, "Sales123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(salesUser, "Sales");
    }

    var defaultUser = new ApplicationUser { UserName = "user@bsr.com", Email = "user@bsr.com" };
    result = await userManager.CreateAsync(defaultUser, "User123!");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(defaultUser, "User");
    }

    var dataSeedService = scope.ServiceProvider.GetRequiredService<DataSeedService>();
    dataSeedService.SeedHomes();
}
