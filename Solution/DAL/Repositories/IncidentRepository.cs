using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly AlMarsadDbContext _dbContext;

        public IncidentRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(Incident incident)
        {
            if (incident.PersonalVictimTestimonies != null)
            {
                foreach (var test in incident.PersonalVictimTestimonies)
                {
                    if(test.Victim != null)
                        _dbContext.Victims.Add(test.Victim);


                    _dbContext.PersonalVictimTestimonies.Add(test);
                }
            }
            await _dbContext.Incidents.AddAsync(incident); 
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<Incident>, int)> GetPageAsync(int skip, int take, string userId, string? searchVictimNationalId)
        {
            if (skip < 0 || take < 0)
                return (new List<Incident>(), 0);

            var query = _dbContext.Incidents
                .Where(i => i.FieldResearcherId == userId);

            if (!string.IsNullOrEmpty(searchVictimNationalId))
            {
                query = query.Where(i =>
                    i.PersonalVictimTestimonies.Any(t =>
                        t.Victim.NationalId == searchVictimNationalId.Trim()));
            }

            var count = await query.CountAsync();

            return (await query
                .Skip(skip)
                .Take(take)
                .ToListAsync(), count);
        }

        public async Task<Incident?> GetByIdAsync(int id)
        {
            return await _dbContext.Incidents.SingleOrDefaultAsync(i => i.Id == id); 
        }

        public async Task<Incident?> GetFullByIdAsync(int id)
        {
            return await _dbContext.Incidents
                .Include(i => i.PersonalVictimTestimonies)
                .ThenInclude(t => t.Victim)
                .Select(i => new Incident
                {
                    Id = i.Id,
                    DateOfOccurrence = i.DateOfOccurrence,
                    CreationDate = i.CreationDate,
                    DetailedDescription = i.DetailedDescription,
                    WitnessCount = i.WitnessCount,
                    WitnessDetails = i.WitnessDetails,
                    AreaName = i.AreaName,
                    AreaClass = i.AreaClass,
                    AreaType = i.AreaType,
                    LocationDescription = i.LocationDescription,
                    LocationLat = i.LocationLat,
                    LocationLng = i.LocationLng,
                    PerpetratorDescription = i.PerpetratorDescription,
                    SensitivityScore = i.SensitivityScore,
                    QuestionnaireJSON = i.QuestionnaireJSON,
                    CityId = i.CityId,
                    FieldResearcherId = i.FieldResearcherId,
                    PersonalVictimTestimonies = i.PersonalVictimTestimonies.Select(t => new PersonalVictimTestimonie
                    {
                        Id = t.Id,
                        IncidentId = t.IncidentId,
                        VictimId = t.VictimId,
                        Victim = new Victim
                        {
                            Id = t.Victim.Id,
                            FirstName = t.Victim.FirstName,
                            LastName = t.Victim.LastName,
                            NationalId = t.Victim.NationalId
                        }
                    }).ToList()
                })
                .SingleOrDefaultAsync(i => i.Id == id);

        }

        public async Task AddRangeOfEvidencesAsync(List<Evidence> evidences)
        {
            await _dbContext.Evidences.AddRangeAsync(evidences);
        }

        public async Task<List<Evidence>> GetEvidencesByIncidentIdAsync(int incidentId)
        {
            return await _dbContext.Evidences.Where(e => e.IncidentId == incidentId).ToListAsync();
        }

        public async Task<List<PersonalVictimTestimonie>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId)
        {
            return await _dbContext.PersonalVictimTestimonies
                .Where(t => t.IncidentId == incidentId)
                .Include(t => t.Victim)
                .ToListAsync(); 
        }

    }
}
