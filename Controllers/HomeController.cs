using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp_album.Models;
using asp_album.Data;
using asp_album.Models.Dtos;

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
        _imageRootPath = $"{env.WebRootPath}\\Album";
    }

    public IActionResult Index()
    {
        var albums = _context.Albums.OrderByDescending(a => a.ReleaseTime).Take(20).ToList();
        return View(albums);
    }

    public IActionResult AlbumCategory([FromQuery] int categoryId)
    {
        //依Cid分類編號取得相簿分類名稱
        var categoryName = _context.AlbumCategories.FirstOrDefault(c => c.Id == categoryId)?.Name ?? "Unknown Category";
        ViewBag.CategoryName = categoryName;
        //依Cid分類編號取得該分類的所有照片，並依發佈時間進行遞減排序
        var albums = _context.Albums
     .Where(a => a.CategoryId == categoryId)
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
             AlbumName = album.AlbumPath,
             ReleaseTime = album.ReleaseTime,
             CategoryName = categoryName,
             MemberName = member.Name ?? "Unknown Member"
         })
     .ToList();
        return View(albums);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
