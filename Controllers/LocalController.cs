
using asp_album.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace asp_album.Controllers
{
    public class LocalController : Controller
    {

        private readonly ILogger<LocalController> _logger;


        public LocalController(ILogger<LocalController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}