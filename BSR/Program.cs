using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<HomeContext>(opt => opt.UseSqlite("Data Source=bsr.db"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HomeContext>();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "{controller=Homes}/{action=Index}/{id?}");

app.Run();
