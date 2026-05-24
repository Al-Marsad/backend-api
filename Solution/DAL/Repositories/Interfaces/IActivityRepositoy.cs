using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IActivityRepositoy: ICreateRepository<Activity>, ISaveRepository
    {
        public Task<(List<Activity>, int)> GetByPageAsync(int skip, int take, string ? searchContent = null, ActivityType? activityType = null);

    }
}
