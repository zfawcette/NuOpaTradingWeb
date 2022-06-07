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
            GenreRepository = new GenreRepository(_db);
        }
        public IGenreRepository GenreRepository { get; private set; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
