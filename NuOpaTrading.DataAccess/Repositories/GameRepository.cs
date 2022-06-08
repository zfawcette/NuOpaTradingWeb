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
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly ApplicationDbContext _db;
        public GameRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }
        public void Update(Game obj)
        {
            _db.Games.Update(obj);
        }
    }
}
