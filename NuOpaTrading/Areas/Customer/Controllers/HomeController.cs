using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.Models;
using System.Diagnostics;
using IGDB;
using IGDB.Models;
using NuOpaTrading.Utilities;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace NuOpaTrading.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.FindByEmailAsync("admin@nuopatrading.com").GetAwaiter().GetResult();
            var gameList = _unitOfWork.WishList.GetAll(u=>u.UserID == user.Id);
            List<Models.Game> games = new List<Models.Game>();
            foreach(var game in gameList)
            {
                var newGame = _unitOfWork.Game.GetFirstOrDefault(u => u.Id == game.GameID);
                games.Add(newGame);
            }
            return View(games);
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
