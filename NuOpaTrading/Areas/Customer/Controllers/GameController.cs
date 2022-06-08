using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.DataAccess.Repositories.IRepositories;

namespace NuOpaTrading.Areas.Customer.Controllers
{
    public class GameController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var games = _unitOfWork.Game.GetAll();
            return View(games);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
