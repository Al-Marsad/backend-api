using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.Basic
{
    public interface IGetByIdRepository<T>
    {
        public Task<T?> GetByIdAsync(int id);

    }
}
