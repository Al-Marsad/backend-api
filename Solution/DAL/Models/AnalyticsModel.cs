namespace DAL.Models
{
    public class AnalyticsModel
    {
        public List<CountByMonthModel> ByMonth { get; set; } = new();
        public List<CountByYearModel> ByYear { get; set; } = new();
        public List<CountByCityModel> ByCity { get; set; } = new();
    }
}
