using IGDB;
using IGDB.Models;
using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using NuOpaTrading.Models;
using NuOpaTrading.Utilities;
using System.Security.Claims;

namespace NuOpaTrading.Areas.Customer.Controllers
{
    [Area("Customer")]
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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<WishList> gameList = _unitOfWork.WishList.GetAll(u=>u.UserID == claim.Value);
            List<Models.Game> games = new List<Models.Game>();
            foreach (var game in gameList)
            {
                var newGame = _unitOfWork.Game.GetFirstOrDefault(u => u.Id == game.GameID);
                games.Add(newGame);
            }
            return View(games);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int? gameId)
        {
            if(gameId != null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var game = igdb.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: "fields id,name,genres,cover.image_id; where id = " + gameId + ";").GetAwaiter().GetResult().FirstOrDefault();

                //Check if Game is in Game Table if so send a reference to that game, if not add it to the table
                var check = _unitOfWork.Game.GetFirstOrDefault(u => u.GameId == game.Id);
                Models.Game gameToAdd;
                if(check == null)
                {
                    string genres = "";
                    if (game.Genres != null)
                    {
                        foreach (var g in game.Genres.Ids)
                        {
                            genres = genres + g + ",";
                        }
                    }
                    var coverSmall = "";
                    if (game.Cover != null)
                    {
                        var artworkImageId = game.Cover.Value.ImageId;
                        coverSmall = IGDB.ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.CoverSmall, retina: false);
                    }
                    else
                    {
                        coverSmall = "https://via.placeholder.com/100x150";
                    }
                    gameToAdd = new()
                    {
                        Title = game.Name,
                        GameId = game.Id,
                        ImageUrl = coverSmall,
                        Genres = genres
                    };
                    _unitOfWork.Game.Add(gameToAdd);
                    _unitOfWork.Save();
                }
                else
                {
                    gameToAdd = check;
                }
                WishList newWishlistItem = new() { GameID = gameToAdd.Id, UserID = claim.Value };
                _unitOfWork.WishList.Add(newWishlistItem);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(Models.Search obj)
        {
            if(obj.Query == null)
            {
                return View();
            }
            var games = igdb.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields id,name,genres,cover.image_id;limit 100;search \"{obj.Query}\";").GetAwaiter().GetResult();
            List<Models.Game> GameList = GetGames(games);
            return View("Create", GameList);
        }

        private List<Models.Game> GetGames(IEnumerable<IGDB.Models.Game>? games)
        {
            List<Models.Game> GameList = new List<Models.Game>();
            foreach (var game in games)
            {
                string genres = "";
                if (game.Genres != null)
                {
                    foreach (var g in game.Genres.Ids)
                    {
                        genres = genres + g + ",";
                    }
                }
                var coverSmall = "";
                if (game.Cover != null)
                {
                    var artworkImageId = game.Cover.Value.ImageId;
                    coverSmall = IGDB.ImageHelper.GetImageUrl(imageId: artworkImageId, size: ImageSize.CoverSmall, retina: false);
                }
                else
                {
                    coverSmall = "https://via.placeholder.com/100x150";
                }
                Models.Game gameModel = new()
                {
                    Title = game.Name,
                    GameId = game.Id,
                    ImageUrl = coverSmall,
                    Genres = genres
                };
                GameList.Add(gameModel);
            }
            return GameList;
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
            var check = _unitOfWork.WishList.GetAll(u => u.GameID == id);
            if(check.Count() == 1)
            {
                _unitOfWork.Game.Remove(game);
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var listToRemove = _unitOfWork.WishList.GetAll(u => u.UserID == claim.Value).FirstOrDefault(u => u.GameID == id);
            _unitOfWork.WishList.Remove(listToRemove);
            _unitOfWork.Save();
            return Json(new { success = true, message = "successful remove" });
        }

        #endregion
    }
}