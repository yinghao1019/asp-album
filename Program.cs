using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("WebDatabase");
builder.Services.AddDbContext<asp_album.Data.ApplicationDBContext>(
    options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("WebDatabase"), ServerVersion.AutoDetect(connectionString));
    }
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = new PathString("/Home/Login");
    options.AccessDeniedPath = new PathString("/Home/NoAuthorization");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/Hello", () => $"Environment:{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")} , MySettings:{app.Configuration["MySettings"]}");
app.Run();
