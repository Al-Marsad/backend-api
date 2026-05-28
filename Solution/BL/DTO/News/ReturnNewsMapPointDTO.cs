namespace BL.DTO.News
{
    public class ReturnNewsMapPointDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public int CityId { get; set; }
        public DateTime? PublishDate { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
    }
}
