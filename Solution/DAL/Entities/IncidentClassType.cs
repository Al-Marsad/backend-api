namespace DAL.Entities
{
    public class IncidentClassType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } 
        
        public virtual List<Question> Questions { get; set; } = new();  
    }
}
