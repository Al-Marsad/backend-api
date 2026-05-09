using BL.DTO.City;
using BL.DTO.Incident;
using DAL.Entities;

namespace BL.Services.Interfaces
{
    public interface IIncidentService
    {
        public Task<ReturnFullIncidentDTO> AddAsync(AddIncidentDTO incidentDTO);

    }
}
