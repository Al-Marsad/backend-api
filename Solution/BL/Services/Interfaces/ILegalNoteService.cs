using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.LegalNote;
using DAL.Repositories.Interfaces;

namespace BL.Services.Interfaces
{
    public interface ILegalNoteService
    {
        public Task<ReturnLegalNoteDTO> AddAsync(string UserId, AddLegalNoteDTO noteDTO);
        public Task<ReturnLegalNoteDTO> UpdateAsync(string UserId, int NoteId, UpdateLegalNoteDTO noteDTO);
        public Task DeleteAsync(string UserId, int NoteId);
        public Task<List<ReturnLegalNoteDTO>> GetByIncident(int IncidentId);
    }
}
