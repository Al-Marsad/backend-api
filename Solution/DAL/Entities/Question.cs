namespace DAL.Entities
{
    public class Question
    {
        public int Id { get; set; } 
        public string QuestionBody { get; set; }

        public virtual List<IncidentClassType> IncidentClassTypes { get; set; } = new();
    }
}
