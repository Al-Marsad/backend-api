using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO.LegalNote;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class LegalNoteService : ILegalNoteService
    {
        private readonly ILegalNotesRepository _legalNotesRepository;
        private readonly IMapper _mapper;

        public LegalNoteService(ILegalNotesRepository legalNotesRepository,
            IMapper mapper)
        {
            _legalNotesRepository = legalNotesRepository;
            _mapper = mapper;
        }

        public async Task<ReturnLegalNoteDTO> AddAsync(string UserId, AddLegalNoteDTO noteDTO)
        {
            var note = _mapper.Map<LegalTeamMemberNote>(noteDTO);
            note.LegalTeamMemberId = UserId;

            await _legalNotesRepository.AddAsync(note);

            await _legalNotesRepository.SaveAsync();

            return _mapper.Map<ReturnLegalNoteDTO>(note);
        }
        public async Task<ReturnLegalNoteDTO> UpdateAsync(string UserId, int NoteId, UpdateLegalNoteDTO noteDTO)
        {
            var note = await _legalNotesRepository.GetByIdAsync(NoteId);

            if (note == null)
                throw new DataNotFoundException($"There is no legal note with id '{NoteId}' found");

            if(note.LegalTeamMemberId != UserId)
                throw new ForbiddenException("You are not allowed to update legal note that you haven't created");

            note.Content = noteDTO.Content;

            await _legalNotesRepository.SaveAsync();

            return _mapper.Map<ReturnLegalNoteDTO>(note);
        }
        public async Task DeleteAsync(string UserId, int NoteId)
        {
            var note = await _legalNotesRepository.GetByIdAsync(NoteId);

            if (note == null)
                throw new DataNotFoundException($"There is no legal note with id '{NoteId}' found");

            if (note.LegalTeamMemberId != UserId)
                throw new ForbiddenException("You are not allowed to delete legal note that you haven't created");

            _legalNotesRepository.Delete(note);

            await _legalNotesRepository.SaveAsync();
        }
        public async Task<List<ReturnLegalNoteDTO>> GetByIncident(int IncidentId)
        {
            var notes = await _legalNotesRepository.GetByIncidentIdAsync(IncidentId);
            
            return _mapper.Map<List<ReturnLegalNoteDTO>>(notes);
        }

    }
}
