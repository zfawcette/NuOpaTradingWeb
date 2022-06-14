using NuOpaTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.DataAccess.Repositories.IRepositories
{
    public interface IWishListRepository : IRepository<WishList>
    {
        void Update(WishList obj);
    }
}
