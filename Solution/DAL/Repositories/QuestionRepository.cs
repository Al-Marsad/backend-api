using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    internal class QuestionRepository : IQuestionRepository
    {
        private readonly AlMarsadDbContext _dbContext;

        public QuestionRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<Question>> GetFullQuestionByIncidentClassTypeAsync(int[] ids)
        {
            var questions = await _dbContext.Questions.Include(q => q.IncidentClassTypes)
                .Where(q => q.IncidentClassTypes.Any(ict => ids.Contains(ict.Id)))
                .ToListAsync();
            
            return questions;
        }

    }
}
