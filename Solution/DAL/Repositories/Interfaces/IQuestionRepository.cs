using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        public Task<List<Question>> GetFullQuestionByIncidentClassTypeAsync(int[] ids);
    }
}
