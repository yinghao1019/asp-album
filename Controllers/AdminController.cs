using asp_album.Data;
using asp_album.Models.Dtos;
using asp_album.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asp_album.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDBContext _context;
        private string _path;
        public AdminController(ApplicationDBContext context, IWebHostEnvironment hostEnvironment, ILogger<AdminController> logger)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
            _logger = logger;
        }
        //Admin/Index，取得所有分類記錄
        public IActionResult Index()
        {

            var categories = from c in _context.AlbumCategories
                             orderby c.Id
                             select new AlbumCategoryDTO
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 CreatedDate = c.CreatedDate,
                                 UpdatedDate = c.UpdatedDate
                             };
            return View(categories);

        }

        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(AlbumCategoryDTO albumCategory)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var category = new AlbumCategoryEntity
                    {
                        Name = albumCategory.Name,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    _context.AlbumCategories.Add(category);
                    _context.SaveChanges();
                    TempData["Success"] = "相簿分類新增成功";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "相簿分類新增失敗";
                    _logger.LogError(ex, "Error creating category");
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(albumCategory);
        }

        //編輯相簿分類
        public IActionResult CategoryEdit(int Cid)
        {

            var categories = _context.AlbumCategories.FirstOrDefault(c => c.Id == Cid);
            if (categories == null)
            {
                return RedirectToAction("Index");
            }


            return View(categories);

        }

        [HttpPost]
        public IActionResult CategoryEdit([FromForm] AlbumCategoriesDTO albumCategoriesDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categories = _context.AlbumCategories.FirstOrDefault(c => c.Id == albumCategoriesDTO.Id);
                    if (categories != null)
                    {
                        categories.Name = albumCategoriesDTO.Name;
                        _context.SaveChanges();
                        TempData["Success"] = "相簿分類修改成功";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating category");
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(albumCategoriesDTO);
        }


        //刪除相簿分類
        public IActionResult CategoryDelete([FromRoute] int Cid)
        {

            var categories = _context.AlbumCategories.FirstOrDefault(c => c.Id == Cid);

            if (categories == null)
            {
                TempData["Error"] = "相簿分類刪除失敗";
                return RedirectToAction("Index");
            }
            var albums = _context.Albums.Where(a => a.CategoryId == Cid);
            foreach (var album in albums)
            {
                var filePath = Path.Combine(_path, album.ImgName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.Albums.RemoveRange(album);
                _context.AlbumCategories.Remove(categories);
                _context.SaveChanges();
            }
            TempData["Success"] = "相簿分類刪除成功";
            return RedirectToAction("Index");
        }

        // 會員列表
        public IActionResult MemberList()
        {

            var members = from m in _context.Members
                          where m.Role != "Admin"
                          orderby m.Id
                          select new MemberQueryDTO
                          {
                              Id = m.Id,
                              Name = m.Name,
                              Role = m.Role,
                              Mail = m.Mail,
                              Uid = m.Uid,
                              CreatedDate = m.CreatedDate,
                              UpdatedDate = m.UpdatedDate
                          };
            return View(members);

        }
        // 會員新增
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
                    TempData["Success"] = "會員新增成功";
                    return RedirectToAction("MemberList");
                }
                catch (Exception e)
                {
                    TempData["Error"] = "會員新增失敗";
                    _logger.LogError(e, "Error creating member");
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(memberCreateDTO);
        }

        // 會員修改
        public IActionResult MemberEdit(string Uid)
        {

            var member = _context.Members.FirstOrDefault(m => m.Uid == Uid);
            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit(MemberEditDTO memberEditDTO)
        {

            if (ModelState.IsValid)
            {
                // TODO 可調整權限
                try
                {
                    var member = _context.Members.FirstOrDefault(m => m.Uid == memberEditDTO.Uid);
                    if (member != null)
                    {
                        member.Name = memberEditDTO.Name;
                        member.Mail = memberEditDTO.Mail;
                        member.UpdatedDate = DateTime.Now;
                        _context.SaveChanges();
                        TempData["Success"] = "會員修改成功";
                        return RedirectToAction("MemberList");
                    }
                }
                catch (Exception e)
                {
                    TempData["Error"] = "會員新增失敗";
                    _logger.LogError(e, "Error creating member");
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(memberEditDTO);
        }

        // 會員刪除
        public IActionResult MemberDelete(string Uid)
        {

            var member = _context.Members.FirstOrDefault(m => m.Uid == Uid);
            if (member == null)
            {
                TempData["Error"] = "會員刪除失敗";
            }
            else
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
                TempData["Success"] = "會員刪除成功";
            }
            return RedirectToAction("MemberList");
        }

    }
}