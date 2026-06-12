using DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    public class ActivityStatsModel
    {
        public int IncidentCreatedCount { get; set; }
        public int IncidentUpdatedCount { get; set; }
        public int IncidentDeletedCount { get; set; }

        public int RequestChangeCount { get; set; }
        public int OtherCount { get; set; }
    }
}
