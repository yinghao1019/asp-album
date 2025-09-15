
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace asp_album.Controllers
{
    public class LocalController : Controller
    {

        private readonly ILogger<LocalController> _logger;
        private readonly IStringLocalizer<LocalController> _localizer;

        public LocalController(ILogger<LocalController> logger, IStringLocalizer<LocalController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            _logger.LogInformation(_localizer.GetString("Hello"));
            ViewBag.hello = _localizer.GetString("Hello");
            return View();
        }
    }
}