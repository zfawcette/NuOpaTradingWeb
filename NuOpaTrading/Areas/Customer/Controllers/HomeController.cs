using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.Models;
using System.Diagnostics;
using IGDB;
using IGDB.Models;
using NuOpaTrading.Utilities;

namespace NuOpaTrading.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IGDBClient igdb = new IGDBClient(SecretInfo.IGDB_PK,SecretInfo.IGDB_SK);

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
