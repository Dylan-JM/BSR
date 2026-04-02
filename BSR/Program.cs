using BSR.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<HomeService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
