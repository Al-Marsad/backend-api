using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBContext;
using DAL.Entities;
using DAL.Enums;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ActivityRepository : IActivityRepositoy
    {
        private readonly AlMarsadDbContext _dbContext;

        public ActivityRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(Activity obj)
        {
            await _dbContext.Activities.AddAsync(obj); 
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<Activity>,int)> GetByPageAsync(int skip, int take, string? searchContent = null, ActivityType? activityType = null)
        {
            if (skip < 0 || take < 0)
                return (new List<Activity>(), 0);

            var query = _dbContext.Activities.AsQueryable();

            if(activityType != null)
            {
                query = query.Where(a => a.Type == activityType);
            }

            if(searchContent != null)
            {
                query = query.Where(a => a.Description.Contains(searchContent.Trim()));
            }

            var totalCount = await query.CountAsync();

            var activities = await query
                .OrderByDescending(a => a.CreationDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (activities, totalCount);
        }

        public async Task<ActivityStatsModel> GetStatsAsync()
        {
            return new ActivityStatsModel
            {
                IncidentCreatedCount = await _dbContext.Activities
                    .CountAsync(a => a.Type == ActivityType.Add),
                IncidentUpdatedCount = await _dbContext.Activities
                    .CountAsync(a => a.Type == ActivityType.Update),
                IncidentDeletedCount = await _dbContext.Activities
                    .CountAsync(a => a.Type == ActivityType.Delete),
                RequestChangeCount = await _dbContext.Activities
                    .CountAsync(a => a.Type == ActivityType.RequestChange),
                OtherCount = await _dbContext.Activities
                    .CountAsync(a => a.Type == ActivityType.Other)
            };
        }


    }
}
