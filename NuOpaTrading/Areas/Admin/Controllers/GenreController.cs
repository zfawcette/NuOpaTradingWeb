using Microsoft.AspNetCore.Mvc;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using NuOpaTrading.Models;

namespace NuOpaTrading.Areas.Admin.Controllers
{
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Genre> genreList = _unitOfWork.GenreRepository.GetAll();
            return View(genreList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            var genre = _unitOfWork.GenreRepository.GetFirstOrDefault(u => u.Id == id);
            if (genre == null)
            {
                return View(new Genre());
            }
            else
            {
                return View(genre);
            }
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genre obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == null || obj.Id == 0)
                {
                    _unitOfWork.GenreRepository.Add(obj);
                }
                else
                {
                    _unitOfWork.GenreRepository.Update(obj);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        //GET
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.GenreRepository.GetFirstOrDefault(u => u.Id == id);
            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(Genre obj)
        {
            _unitOfWork.GenreRepository.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
