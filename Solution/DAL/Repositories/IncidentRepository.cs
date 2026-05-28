using System.Linq;
using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<(List<Incident>, int)> GetIncidentsByPageAndUserIdAsync(int skip, int take, string userId, string role,
            int? cityId, string? searchVictimNationalId, bool OrderByDateOfOccurence,
            bool? DocumentationConsent, bool? PublicationConsent, int? Sensitivity)
        {
            if (skip < 0 || take < 0)
                return (new List<Incident>(), 0);

            var query = _dbContext.Incidents.AsQueryable();
            
            if (role == "FIELD_RESEARCHER") {
                    query = query.Where(i => i.FieldResearcherId == userId); 
            } else if (role == "LEGAL_TEAM_MEMBER")
            {
                query = query.Where(i => i.LegalTeamMemberId == userId);
            }

            if (cityId != null)
            {
                query = query.Where(i => i.CityId == cityId);
            }

            if (DocumentationConsent != null)
            {
                query = query.Where(i => i.DocumentationConsent == DocumentationConsent.Value);
            }

            if (PublicationConsent != null)
            {
                query = query.Where(i => i.PublicationConsent == PublicationConsent.Value);
            }

            if (Sensitivity != null)
            {
                query = query.Where(i => i.SensitivityScore == Sensitivity);
            }

            if (!string.IsNullOrEmpty(searchVictimNationalId))
            {
                query = query.Where(i =>
                    i.PersonalVictimTestimonies.Any(t =>
                        t.Victim.NationalId == searchVictimNationalId.Trim()));
            }

            if (OrderByDateOfOccurence)
            {
                query = query.OrderByDescending(i => i.DateOfOccurrence);
            } else
            {
                query = query.OrderByDescending(i => i.CreationDate);

            }

            var count = await query.CountAsync();

            return (await query
                .Skip(skip)
                .Take(take)
                .ToListAsync(), count);
        }

        public async Task<(List<Incident>, int)> GetAllIncidentsByPageAsync(int skip, int take, int? cityId
            , bool OrderByDateOfOccurence, bool? DocumentationConsent, bool? PublicationConsent, int? Sensitivity)
        {
            if (skip < 0 || take < 0)
                return (new List<Incident>(), 0);

            var query = _dbContext.Incidents.AsQueryable();

            if(cityId != null)
            {
                query = query.Where(i => i.CityId == cityId);
            }

            if (DocumentationConsent != null)
            {
                query = query.Where(i => i.DocumentationConsent == DocumentationConsent.Value);

            }

            if (PublicationConsent != null)
            {
                query = query.Where(i => i.PublicationConsent == PublicationConsent.Value);

            }

            if (Sensitivity != null)
            {
                query = query.Where(i => i.SensitivityScore == Sensitivity);
            }

            if (OrderByDateOfOccurence)
            {
                query = query.OrderByDescending(i => i.DateOfOccurrence);
            }
            else
            {
                query = query.OrderByDescending(i => i.CreationDate);
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

        public async Task<Incident?> GetWithTestimoniesOnlyById(int id)
        {
            return await _dbContext.Incidents
               .Include(i => i.PersonalVictimTestimonies)
               .SingleOrDefaultAsync(i => i.Id == id);
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
