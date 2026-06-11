namespace BL.DTO.Stats
{
    public class MyIncidentStatsDTO
    {
        public int PendingReviewCount { get; set; }
        public int UnderReviewCount { get; set; }
        public int ReviewedCount { get; set; }
        public int SentToManagerCount { get; set; }
    }
}
