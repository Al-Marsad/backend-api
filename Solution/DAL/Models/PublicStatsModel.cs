namespace DAL.Models
{
    public class PublicStatsModel
    {
        public int TotalIncidents { get; set; }
        public int RegionsAffected { get; set; }
        public int ReportsThisMonth { get; set; }
        public int PendingReview { get; set; }
    }
}
