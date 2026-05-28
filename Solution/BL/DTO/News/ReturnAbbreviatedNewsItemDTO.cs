namespace BL.DTO.News
{
    public class ReturnAbbreviatedNewsItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CityId { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
    }
}
