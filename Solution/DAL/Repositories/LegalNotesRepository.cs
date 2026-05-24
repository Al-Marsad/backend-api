using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LegalNotesRepository : ILegalNotesRepository
    {
        private readonly AlMarsadDbContext _dbContext;

        public LegalNotesRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(LegalTeamMemberNote obj)
        {
            await _dbContext.LegalTeamMemberNotes.AddAsync(obj);
        }

        public async void Delete(LegalTeamMemberNote entity)
        {
            _dbContext.LegalTeamMemberNotes.Remove(entity);
        }

        public async Task<LegalTeamMemberNote?> GetByIdAsync(int id)
        {
            return await _dbContext.LegalTeamMemberNotes.SingleOrDefaultAsync(i => i.Id == id); 
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public Task<List<LegalTeamMemberNote>> GetByIncidentIdAsync(int incidentId)
        {
            return _dbContext.LegalTeamMemberNotes.Where(n => n.IncidentId == incidentId).ToListAsync();
        }


    }
}
