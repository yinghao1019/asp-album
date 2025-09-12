
using Microsoft.AspNetCore.Mvc;

namespace asp_album.Controllers
{
    public class LocalController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}