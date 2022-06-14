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
    public class WishListRepository : Repository<WishList>, IWishListRepository
    {
        private readonly ApplicationDbContext _db;
        public WishListRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }
        public void Update(WishList obj)
        {
            _db.WishLists.Update(obj);
        }
    }
}
