using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp_album.Models;
using asp_album.Data;
using asp_album.Models.Dtos;
using asp_album.Models.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BCrypt.Net;

namespace asp_album.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context;
    private readonly string _imageRootPath;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context, IWebHostEnvironment env)
    {
        _logger = logger;
        _context = context;
        _imageRootPath = $"{env.WebRootPath}/album";
    }

    public IActionResult Index()
    {
        var albums = _context.Albums.OrderByDescending(a => a.ReleaseTime).Join(
         _context.Members,
         album => album.MemberId,
         member => member.Id,
         (album, member) => new AlbumQueryDTO
         {
             Id = album.Id,
             Title = album.Title,
             Description = album.Description,
             AlbumName = album.ImgName,
             ReleaseTime = album.ReleaseTime,
             MemberName = member.Name ?? "Unknown Member"
         }).Take(20).ToList();
        return View(albums);
    }

    public IActionResult AlbumCategory([FromRoute] int id)
    {
        //依Cid分類編號取得相簿分類名稱
        var categoryName = _context.AlbumCategories.FirstOrDefault(c => c.Id == id)?.Name ?? "Unknown Category";
        ViewBag.CategoryName = categoryName;
        //依Cid分類編號取得該分類的所有照片，並依發佈時間進行遞減排序
        var albums = _context.Albums
     .Where(a => a.CategoryId == id)
     .OrderByDescending(a => a.ReleaseTime)
     .Join(
         _context.Members,
         album => album.MemberId,
         member => member.Id,
         (album, member) => new AlbumQueryDTO
         {
             Id = album.Id,
             Title = album.Title,
             Description = album.Description,
             AlbumName = album.ImgName,
             ReleaseTime = album.ReleaseTime,
             CategoryName = categoryName,
             MemberName = member.Name ?? "Unknown Member"
         })
     .ToList();
        return View(albums);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(MemberLoginDTO loginDto)
    {
        if (ModelState.IsValid)
        {
            var member = _context.Members
                .FirstOrDefault(m => m.Uid == loginDto.Uid);

            // 使用 BCrypt.Verify 來驗證密碼
            if (member != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, member.Password))
            {
                IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, member.Uid),
                new Claim(ClaimTypes.Role, member.Role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["Success"] = "登入成功";
                return RedirectToAction("Index");
            }
        }

        TempData["Error"] = "帳密錯誤，請重新檢查";
        return View();
    }

    //register member
    public IActionResult MemberCreate()
    {
        return View();
    }

    [HttpPost]
    public IActionResult MemberCreate(MemberCreateDTO memberCreateDTO)
    {

        if (ModelState.IsValid)
        {

            try
            {
                var member = new MemberEntity
                {
                    Uid = memberCreateDTO.Uid,
                    Password = BCrypt.Net.BCrypt.HashPassword(memberCreateDTO.Password),
                    Name = memberCreateDTO.Name,
                    Mail = memberCreateDTO.Mail,
                    Role = "Member",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                _context.Members.Add(member);
                _context.SaveChanges();
                TempData["Success"] = "會員註冊成功";
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                TempData["Error"] = "會員註冊失敗";
                _logger.LogError(e, "Error creating member");
                ModelState.AddModelError(string.Empty, e.Message);
            }
        }
        return View(memberCreateDTO);
    }

    public IActionResult NoAuthorization()
    {
        return View();
    }
    //Home/Logout，登出作業
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
