using NuOpaTrading.DataAccess.Data;
using NuOpaTrading.DataAccess.Repositories.IRepositories;
using NuOpaTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.DataAccess.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _db;
        public GenreRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }
        public void Update(Genre obj)
        {
            _db.Genres.Update(obj);
        }
    }
}
