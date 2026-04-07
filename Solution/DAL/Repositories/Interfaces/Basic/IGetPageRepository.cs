using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.Basic
{
    public interface IGetPageRepository<T>
    {
        public Task<List<T>> GetPageAsync(int Skip, int Take, string userId);
    }
}
