using DAL.Entities;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface ILegalNotesRepository: 
        ICreateRepository<LegalTeamMemberNote>, 
        IDeleteRepository<LegalTeamMemberNote>, 
        IGetByIdRepository<LegalTeamMemberNote>, 
        ISaveRepository    
    {
        public Task<List<LegalTeamMemberNote>> GetByIncidentIdAsync(int incidentId);

    }
}
