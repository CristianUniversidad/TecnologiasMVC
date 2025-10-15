using System.Diagnostics;
using hotelResort.Models;
using Microsoft.AspNetCore.Mvc;

namespace hotelResort.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Habitaciones()
        {
            return View();
        }
        public IActionResult Booking()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult registro()
        {
            return View();
        }

        public IActionResult about_us()
        {
            return View();
        }

        public IActionResult resumen()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
