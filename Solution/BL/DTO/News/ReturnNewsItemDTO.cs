namespace BL.DTO.News
{
    public class ReturnNewsItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public DateTime WritingDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublished { get; set; }
        public string WrittenById { get; set; }
        public string WrittenByName { get; set; }
        public ReturnNewsIncidentLocationDTO IncidentLocation { get; set; }
    }
}
