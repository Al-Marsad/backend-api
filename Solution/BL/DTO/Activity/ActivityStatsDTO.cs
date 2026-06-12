namespace BL.DTO.Activity
{
    public class ActivityStatsDTO
    {
        public int IncidentCreatedCount { get; set; }
        public int IncidentUpdatedCount { get; set; }
        public int IncidentDeletedCount { get; set; }

        public int RequestChangeCount { get; set; }
        public int OtherCount { get; set; }
    }
}
