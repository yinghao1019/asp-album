using System.Globalization;
using asp_album.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();
// Localization
//TODO fix i18n
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var currentCulture = CultureInfo.CurrentCulture;
    var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("zh-TW"),
            new CultureInfo("en"),
        };
    options.DefaultRequestCulture = new RequestCulture(currentCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new IRequestCultureProvider[]

{
    new QueryStringRequestCultureProvider(),
        new RouteDataRequestCultureProvider()
        {
            // Route 參數名稱
            RouteDataStringKey = "culture",
            UIRouteDataStringKey = "culture"
        },

    };

    // 在回應標頭中加入 Content-Language 標頭，告知用戶端此份 HTTP 內容的語言為何
    options.ApplyCurrentCultureToResponseHeaders = true;
});


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

}
app.UseMiddleware<ExceptionHandlingMiddleware>();
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
