namespace DAL.Models
{
    public class MyIncidentStatsModel
    {
        public int PendingReviewCount { get; set; }
        public int UnderReviewCount { get; set; }
        public int ReviewedCount { get; set; }
        public int SentToManagerCount { get; set; }
    }
}
