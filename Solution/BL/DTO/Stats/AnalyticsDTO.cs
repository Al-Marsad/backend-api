namespace BL.DTO.Stats
{
    public class AnalyticsDTO
    {
        public List<CountByMonthDTO> ByMonth { get; set; } = new();
        public List<CountByYearDTO> ByYear { get; set; } = new();
        public List<CountByCityDTO> ByCity { get; set; } = new();
    }
}
