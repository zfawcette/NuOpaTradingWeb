using IGDB;
using IGDB.Models;
using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using NuOpaTrading.Utilities;

namespace NuOpaTrading.Areas.Customer.Controllers
{
    public class GameController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        IGDBClient igdb = new IGDBClient(SecretInfo.IGDB_PK, SecretInfo.IGDB_SK);
        public GameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Models.Game> games = _unitOfWork.Game.GetAll();
            return View(games);
        }

        public IActionResult Create()
        {
                //Create view that takes in the queried result as its parameter
                Task<Game[]>? games = igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name,genres; limit 25;");
                List<Models.Game> GameList = new List<Models.Game>();
                string genres = "";
                
                foreach (var game in games.Result)
                {
                    if(game.Genres != null)
                    {
                        foreach (var g in game.Genres.Ids)
                        {
                            genres = genres + g + ",";
                        }
                    }
                    Models.Game gameModel = new()
                    {
                        Title = game.Name,
                        GameId = game.Id,
                        ImageUrl = "",
                        Genres = genres
                    };
                    GameList.Add(gameModel);
                }
                return View(GameList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int? gameId)
        {
            if(gameId != null)
            {
                var game = igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id,name,genres,cover.image_id; where id = " + gameId + ";");
                string genres = "";
                if (game.Result.First().Genres != null)
                {
                    foreach (var g in game.Result.First().Genres.Ids)
                    {
                        genres = genres + g + ",";
                    }
                }
                var artworkImageId = game.Result.First().Cover.Value.ImageId;
                var coverSmall = IGDB.ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.CoverSmall, retina: false);
                Models.Game newGame = new() { Title = game.Result.First().Name, GameId = game.Result.First().Id, ImageUrl = coverSmall, Genres = genres };
                _unitOfWork.Game.Add(newGame);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(NuOpaTrading.Models.Search obj)
        {
            if(obj.Query == null)
            {
                return View();
            }
            var games = igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields id,name,genres,cover.image_id;limit 25;search \"{obj.Query}\";");
            List<Models.Game> GameList = new List<Models.Game>();
            foreach (var game in games.Result)
            {
                string genres = "";
                if (game.Genres != null)
                {
                    foreach (var g in game.Genres.Ids)
                    {
                        genres = genres + g + ",";
                    }
                }
                var artworkImageId = game.Cover.Value.ImageId;
                var coverSmall = IGDB.ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.CoverSmall, retina: false);
                Models.Game gameModel = new()
                {
                    Title = game.Name,
                    GameId = game.Id,
                    ImageUrl = coverSmall,
                    Genres = genres
                };
                GameList.Add(gameModel);
            }
            return View("Create", GameList);
        }
        #region API CALLS
        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            var game = _unitOfWork.Game.GetFirstOrDefault(u=>u.Id == id);
            if(game == null)
            {
                return Json(new {success = false, message = "Error Removing"});
            }

            _unitOfWork.Game.Remove(game);
            _unitOfWork.Save();
            return Json(new { success = true, message = "successful remove" });
        }

        #endregion
    }
}