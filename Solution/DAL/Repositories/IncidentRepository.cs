using System.Linq;
using DAL.DBContext;
using DAL.Entities;
using DAL.Enums;
using DAL.Models;
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
            bool? DocumentationConsent, bool? PublicationConsent, bool? PreventModification, int? Sensitivity)
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

            if (PreventModification != null)
            {
                query = query.Where(i => i.PreventModification == PreventModification.Value);

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

        public async Task<(List<Incident>, int)> GetAllIncidentsByPageAsync(int skip, int take, int? cityId, string? searchVictimNationalId
            , bool OrderByDateOfOccurence, bool? DocumentationConsent, bool? PublicationConsent, bool? PreventModification, int? Sensitivity)
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

            if (PreventModification != null)
            {
                query = query.Where(i => i.PreventModification == PreventModification.Value);

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

        public async Task<List<Evidence>> GetEvidencesByIncidentIdAsync(int incidentId, EvidenceType? type = null)
        {
            var query = _dbContext.Evidences.Where(e => e.IncidentId == incidentId);
            
            if (type != null)
            {
                query = query.Where(e => e.Type == type.Value);
            }
            
            return await query.ToListAsync();
        }

        public async Task<List<PersonalVictimTestimonie>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId)
        {
            return await _dbContext.PersonalVictimTestimonies
                .Where(t => t.IncidentId == incidentId)
                .Include(t => t.Victim)
                .ToListAsync(); 
        }

        public async Task<IncidentStatsModel> GetStatsAsync()
        {
            return new IncidentStatsModel
            {
                PendingPublicationCount = await _dbContext.Incidents
                    .CountAsync(i => i.DocumentationConsent && !i.PublicationConsent),
                PublishedCount = await _dbContext.Incidents
                    .CountAsync(i => i.PublicationConsent),
                LockedUnpublishedCount = await _dbContext.Incidents
                    .CountAsync(i => i.PreventModification && !i.PublicationConsent),
                TotalCount = await _dbContext.Incidents.CountAsync()
            };
        }

        public async Task<MyIncidentStatsModel> GetMyStatsAsync(string userId)
        {
            return new MyIncidentStatsModel
            {
                PendingReviewCount = await _dbContext.Incidents
                    .CountAsync(i => i.LegalTeamMemberId == null &&
                                     !i.DocumentationConsent &&
                                     !i.PublicationConsent),
                UnderReviewCount = await _dbContext.Incidents
                    .CountAsync(i => i.LegalTeamMemberId == userId &&
                                     !i.DocumentationConsent &&
                                     !i.PublicationConsent),
                ReviewedCount = await _dbContext.Incidents
                    .CountAsync(i => i.LegalTeamMemberId == userId &&
                                     i.PreventModification &&
                                     !i.DocumentationConsent &&
                                     !i.PublicationConsent),
                SentToManagerCount = await _dbContext.Incidents
                    .CountAsync(i => i.LegalTeamMemberId == userId &&
                                     i.DocumentationConsent &&
                                     !i.PublicationConsent)
            };
        }

        public async Task<IncidentBrowseStatsModel> GetBrowseStatsAsync()
        {
            return new IncidentBrowseStatsModel
            {
                TotalCount = await _dbContext.Incidents.CountAsync(),
                DocumentedCount = await _dbContext.Incidents
                    .CountAsync(i => i.DocumentationConsent),
                PublishedCount = await _dbContext.Incidents
                    .CountAsync(i => i.PublicationConsent)
            };
        }

        public async Task<PublicStatsModel> GetPublicStatsAsync()
        {
            var now = DateTime.UtcNow;

            return new PublicStatsModel
            {
                TotalIncidents = await _dbContext.Incidents
                    .CountAsync(),
                RegionsAffected = await _dbContext.Incidents
                    .Select(i => i.CityId)
                    .Distinct()
                    .CountAsync(),
                ReportsThisMonth = await _dbContext.Incidents
                    .CountAsync(i => i.CreationDate.Year == now.Year &&
                                     i.CreationDate.Month == now.Month),
                PendingReview = await _dbContext.Incidents
                    .CountAsync(i => !i.DocumentationConsent &&
                                     !i.PublicationConsent)
            };
        }

        public async Task<AnalyticsModel> GetAnalyticsAsync()
        {
            var publishedIncidents = _dbContext.Incidents.AsQueryable();

            return new AnalyticsModel
            {
                ByMonth = await publishedIncidents
                    .GroupBy(i => new { i.CreationDate.Year, i.CreationDate.Month })
                    .Select(g => new CountByMonthModel
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Count = g.Count()
                    })
                    .OrderByDescending(i => i.Count)
                    .ToListAsync(),
                ByYear = await publishedIncidents
                    .GroupBy(i => i.CreationDate.Year)
                    .Select(g => new CountByYearModel
                    {
                        Year = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(i => i.Count)
                    .ToListAsync(),
                ByCity = await publishedIncidents
                    .GroupBy(i => new
                    {
                        i.CityId,
                        i.City.ArabicName,
                        i.City.EnglishName
                    })
                    .Select(g => new CountByCityModel
                    {
                        CityId = g.Key.CityId,
                        ArabicName = g.Key.ArabicName,
                        EnglishName = g.Key.EnglishName,
                        Count = g.Count()
                    })
                    .OrderByDescending(i => i.Count)
                    .ToListAsync()
            };
        }

    }
}
