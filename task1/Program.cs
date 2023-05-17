using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using task1.Data;
using task1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");

var AppConfiguration = configuration.Build();

var connectionString = AppConfiguration.GetSection("ConnectionStrings:DefaultConnection").Value;

builder.Services.AddDbContext<UrlShortenerContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<UrlShortenerContext>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
       name: "delete",
       pattern: "Url/delete/{id}",
       defaults: new { controller = "Url", action = "Delete" }
   );

app.Run();
