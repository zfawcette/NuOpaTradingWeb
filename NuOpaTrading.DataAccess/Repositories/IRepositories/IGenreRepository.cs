using NuOpaTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuOpaTrading.DataAccess.Repositories.IRepositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        void Update(Genre obj);
    }
}
