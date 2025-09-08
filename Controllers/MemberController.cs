
using asp_album.Data;
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

        public IActionResult AlbumCategory([FromRoute] int id)
        {
            ViewBag.CategoryName = _context.AlbumCategories
              .FirstOrDefault(m => m.Id == id)
              .Name;
            var memberUid = HttpContext.User.Identity.Name;
            var member = _context.Members.FirstOrDefault(m => m.Uid == memberUid);
            var albums = _context.Albums.Where(a => a.CategoryId == id && a.MemberId == member.Id).OrderByDescending(m => m.Id).ToList();
            return View(albums);
        }

    }
}