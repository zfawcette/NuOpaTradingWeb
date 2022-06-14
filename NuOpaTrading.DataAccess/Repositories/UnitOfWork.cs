using NuOpaTrading.DataAccess.Data;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            Game = new GameRepository(_db);
            WishList = new WishListRepository(_db);
        }
        public IGenreRepository Genre { get; private set; }
        public IGameRepository Game { get; private set; }
        public IWishListRepository WishList { get; private set; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
