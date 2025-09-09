
using asp_album.Data;
using asp_album.Models.Dtos;
using asp_album.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asp_album.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;
        private readonly ApplicationDBContext _context;
        private readonly string _imageRootPath;

        public MemberController(ILogger<MemberController> logger, ApplicationDBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _imageRootPath = $"{env.WebRootPath}\\Album";
        }

        public IActionResult Index()
        {
            var categories = _context.AlbumCategories.OrderByDescending(m => m.Id).ToList();
            return View(categories);
        }

        public IActionResult AlbumList([FromRoute] int id)
        {
            ViewBag.CategoryName = _context.AlbumCategories
              .FirstOrDefault(category => category.Id == id)?
              .Name;
            var memberId = getCurrentUserId();
            var albums = _context.Albums.Where(a => a.CategoryId == id && a.MemberId == memberId).OrderByDescending(m => m.Id).ToList();
            return View(albums);
        }


        public IActionResult AlbumDelete(int albumId)
        {
            var memberId = getCurrentUserId();
            var album = _context.Albums.Where(album => album.Id == albumId && album.MemberId == memberId).FirstOrDefault();

            if (album == null)
            {
                TempData["Failed"] = "刪除失敗";
            }
            else
            {
                _context.Remove(album);
                _context.SaveChanges();
                TempData["Success"] = "刪除成功";
            }
            return RedirectToAction("AlbumList");
        }
        public IActionResult AlbumUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AlbumUpload(AlbumCreateDTO albumCreateDTO)
        {

            if (ModelState.IsValid)
            {
                if (albumCreateDTO.Album != null && albumCreateDTO.Album.Length > 0)
                {

                    // create file
                    string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                    string savePath = $"{_imageRootPath}//{fileName}";
                    using (var stream = new FileStream(savePath, FileMode.CreateNew))
                    {
                        await albumCreateDTO.Album.CopyToAsync(stream);
                    }
                    // record data
                    var currentDate = DateTime.Now;
                    var albumEntity = new AlbumEntity()
                    {
                        CategoryId = albumCreateDTO.CategoryId,
                        Title = albumCreateDTO.Title,
                        Description = albumCreateDTO.Description,
                        ImgName = fileName,
                        MemberId = getCurrentUserId(),
                        ReleaseTime = currentDate,
                        CreatedDate = currentDate,
                        UpdatedDate = currentDate
                    };

                    _context.Add(albumEntity);
                    _context.SaveChanges();

                    TempData["Success"] = "照片上傳成功";
                    return RedirectToAction("AlbumList", new { id = albumCreateDTO.CategoryId });
                }
            }
            TempData["Error"] = "資料無法上傳，請記得上傳照片並檢視資料";
            return View(albumCreateDTO);
        }


        private int getCurrentUserId()
        {
            var memberUid = HttpContext.User.Identity.Name;
            var member = _context.Members.FirstOrDefault(m => m.Uid == memberUid);
            return member.Id;
        }
    }
}