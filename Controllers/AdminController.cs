using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_album.Data;
using asp_album.Models.Dtos;
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

        //Admin/Index，取得所有分類記錄
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
    }
}