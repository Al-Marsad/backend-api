namespace DAL.Models
{
    public class IncidentStatsModel
    {
        public int PendingPublicationCount { get; set; }
        public int PublishedCount { get; set; }
        public int LockedUnpublishedCount { get; set; }
        public int TotalCount { get; set; }
    }
}
